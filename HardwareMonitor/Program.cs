using System;
using OpenHardwareMonitor.Hardware;

// Code adapted from: https://performancepsu.com/open-hardware-monitor-source-code-dll-with-c/

namespace HardwareMonitor
{
    internal class Program
    {
       /*
        * Define variables to hold stats
        */

        // CPU Temp
        static float cpuTemp = 0;

        /**
         * Init OpenHardwareMonitor.dll Computer Object
         */
        static Computer c = new Computer()
        {
            CPUEnabled = true,
        };

        /**
         * Pulls data from OHM
         */
        static void ReportSystemInfo()
        {
            foreach (var hardware in c.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    // only fire the update when found
                    hardware.Update();

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Package"))
                        {
                            printSensorData(sensor, ref cpuTemp, "cpuTemp");
                        }
                    }
                }
            }

        }

        private static void printSensorData(ISensor sensor, ref float sensorVar, string label)
        {
            sensorVar = sensor.Value.GetValueOrDefault();
            
            Console.ResetColor();

            if(sensorVar <= 70 && sensorVar > 0) { Console.ForegroundColor = ConsoleColor.Green; }
            else if (sensorVar > 70 && sensorVar > 0) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else if (sensorVar > 80 ) { Console.ForegroundColor = ConsoleColor.Red; }
            
            // print to console
            label += ": ";
            Console.Write("\r\x1b[1m{0}° C  (Press 'Esc' to quit.)\x1b[0m", label + sensorVar);
        }


        static void Main(string[] args)
        {

            Console.Clear();

            c.Open();
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                ReportSystemInfo();
            }

            Environment.Exit(0);
        }
    }
}
