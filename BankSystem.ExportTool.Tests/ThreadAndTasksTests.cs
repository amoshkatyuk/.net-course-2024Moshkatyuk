using BankSystem.App.Services;
using BankSystem.Domain.Models;
using ExportTool;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.ExportTool.Tests
{
    public class ThreadAndTasksTests
    {
        private readonly ExportService<Client> _exportService;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly string _testJsonDirectory = "TestJsonDirectory";
        private readonly string _testJsonFileName = "testJsonClients";
        private readonly long _maxFileSize = 10000;

        private readonly List<Client> _clientList = new List<Client>();
        private readonly object _locker = new object();
        private bool _addingCompleted = false;
        private int _activeAddingThreads = 0;

        public ThreadAndTasksTests()
        {
            _exportService = new ExportService<Client>();
            _testDataGenerator = new TestDataGenerator();

            if (!Directory.Exists(_testJsonDirectory))
                Directory.CreateDirectory(_testJsonDirectory);
        }

        private void AddClientsInMultipleThreads() 
        {
            int threadCount = 10;
            int clientsPerThread = 5;

            for (int i = 0; i < threadCount; i++)
            {
                Interlocked.Increment(ref _activeAddingThreads);
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    for (int j = 0; j < clientsPerThread; j++)
                    {
                        var client = _testDataGenerator.GenerateClient();

                        lock (_locker)
                        {
                            _clientList.Add(client);
                        }
                    }

                    if (Interlocked.Decrement(ref _activeAddingThreads) == 0)
                    {
                        lock (_locker)
                        {
                            _addingCompleted = true;
                        }
                    }
                });
            }
        }

        private void ProcessClientQueue()
        {
            int fileCounter = 1;
            string currentFileName = $"{_testJsonFileName}_{fileCounter}.json";
            long currentFileSize = 0;
            List<Client> clients = new List<Client>();

            while (true)
            {
                Client client = null;

                lock (_locker)
                {
                    if (_clientList.Count > 0)
                    {
                        client = _clientList[0];
                        _clientList.RemoveAt(0);
                    }
                    else if (_addingCompleted)
                    {
                        break;
                    }
                }

                if (client == null)
                {
                    Thread.Sleep(50);
                    continue;
                }

                clients.Add(client);

                string jsonContent = JsonConvert.SerializeObject(clients, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                long newContentSize = Encoding.UTF8.GetByteCount(jsonContent) + Environment.NewLine.Length;

                if (currentFileSize + newContentSize > _maxFileSize)
                {
                    File.WriteAllText(Path.Combine(_testJsonDirectory, currentFileName), jsonContent);

                    fileCounter++;
                    currentFileName = $"{_testJsonFileName}_{fileCounter}.json";
                    currentFileSize = 0;
                    clients.Clear();
                }
                else
                {
                    currentFileSize += newContentSize;
                }
            }

            if (clients.Count > 0)
            {
                string finalJsonContent = JsonConvert.SerializeObject(clients, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                File.WriteAllText(Path.Combine(_testJsonDirectory, currentFileName), finalJsonContent);
            }
        }


        [Fact]
        public void PipelineProcessingClientsShouldExportToFiles()
        {
            AddClientsInMultipleThreads();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                ProcessClientQueue();
            });

            while (true)
            {
                lock (_locker)
                {
                    if (_addingCompleted && _clientList.Count == 0)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            }

            for (int i = 1; i <= 2; i++)
            {
                string filePath = Path.Combine(_testJsonDirectory, $"{_testJsonFileName}_{i}.json");

                Assert.True(File.Exists(filePath), $"Файл {filePath} должен существовать.");

                List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText(filePath));
                Assert.NotNull(clients);
                Assert.NotEmpty(clients);
            }
        }

        [Fact]
        public void ParallelCreditingOfMoneyToTheAccountShouldReturnTheExpectedAmount()
        {
            var account = new Account { Amount = 0 };
            int increments = 10;
            decimal creditAmount = 100m;
            object _locker = new object();

            void CreditAccount()
            {
                for (int i = 0; i < increments; i++)
                {
                    lock (_locker)
                    {
                        account.Amount += creditAmount;
                    }
                }
            }

            var countdownEvent = new CountdownEvent(2);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                CreditAccount();
                countdownEvent.Signal();
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                CreditAccount();
                countdownEvent.Signal();
            });

            countdownEvent.Wait();

            decimal expectedTotal = creditAmount * increments * 2;
            Assert.Equal(expectedTotal, account.Amount);
        }
    }
}
