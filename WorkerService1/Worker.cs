using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _filePath = "C:\\times\\times.txt"; // Saatlerin bulunduðu dosya yolu
        private readonly string _outputDirectory = "D:\\saat-text"; // Çýktý dosyalarýnýn oluþturulacaðý klasör

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker service is running.");

                try
                {
                    var times = await ReadTimesFromFileAsync(_filePath);

                    var currentTime = DateTime.Now.ToString("HH:mm");

                    if (times.Contains(currentTime))
                    {
                        await PrependTextToFileAsync(currentTime);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing the worker service.");
                }

                await Task.Delay(60000, stoppingToken); // Her dakika kontrol eder
            }

            _logger.LogInformation("Worker service is stopping.");
        }

        private async Task<List<string>> ReadTimesFromFileAsync(string filePath)
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                return lines.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading times from file.");
                return new List<string>();
            }
        }

        private async Task PrependTextToFileAsync(string time)
        {
            try
            {
                var fileName = $"{DateTime.Now:yyyy-MM-dd}.txt"; // Günlük dosya adý
                var filePath = Path.Combine(_outputDirectory, fileName);

                if (!Directory.Exists(_outputDirectory))
                {
                    Directory.CreateDirectory(_outputDirectory);
                }

                var newLogEntry = $"Merhaba Canberk Saat þuan {time}";
                string existingContent = "";

                if (File.Exists(filePath))
                {
                    existingContent = await File.ReadAllTextAsync(filePath);
                }

                using (var writer = new StreamWriter(filePath, false)) // false, dosyanýn üzerine yazmak için
                {
                    await writer.WriteLineAsync(newLogEntry);
                    await writer.WriteLineAsync(existingContent);
                }

                _logger.LogInformation($"Time {time} appended to file: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error appending text to file.");
            }
        }
    }
}
