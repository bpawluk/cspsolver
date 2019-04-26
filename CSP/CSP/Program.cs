using CSP.Input;
using CSP.Output;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CSP
{
    public class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config(false, VariableOrdering.None, ValueOrdering.None);
            //FutoRun(@"data\futoshiki\test_futo_5_1.txt", @"Output\Futoshiki_5_1.html", config);
            SkyRun(@"data\skyscraper\test_sky_4_0.txt", @"Output\Skyscraper_x_x.html", config);
        }

        public static void RunAll(Config config)
        {
            try
            {
                string[] skyscraperPaths = Directory.GetFiles(@"data /skyscraper");
                foreach (string filePath in skyscraperPaths)
                {
                    string[] path = filePath.Split(@"/");
                    string outputPath = @"Output/" + path[path.Length - 1].Split('.')[0] + ".html";
                    Console.WriteLine("Working on: " + filePath);
                    SkyRun(filePath, outputPath, config, true);
                }
                string[] futoshikiPaths = Directory.GetFiles(@"data/futoshiki");
                foreach (string filePath in futoshikiPaths)
                {
                    string[] path = filePath.Split(@"/");
                    string outputPath = @"Output/" + path[path.Length - 1].Split('.')[0] + ".html";
                    Console.WriteLine("Working on: " + filePath);
                    FutoRun(filePath, outputPath, config, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                SendMail("238572@student.pwr.edu.pl", "ERROR OCCURRED!", e.ToString());
            }
        }

        public static void FutoRun(string inputPath, string outputPath, Config config, bool sendMail = false)
        {
            FutoshikiParser fp = new FutoshikiParser();
            var parseResult = fp.ParseCSP(inputPath);
            Problem<int> csp = new Problem<int>(parseResult.Item1, parseResult.Item2);
            Solution<int> solution = csp.Solve(config);
            FutoshikiWriter fw = new FutoshikiWriter(csp, solution);
            fw.WriteSolution(outputPath);
            Console.WriteLine("Job Done! Path to result: " + outputPath);
            if(sendMail) SendMail("238572@student.pwr.edu.pl", "Job Done!", "Path to result: " + outputPath);
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(outputPath)
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public static void SkyRun(string inputPath, string outputPath, Config config, bool sendMail = false)
        {
            SkyscraperParser sp = new SkyscraperParser();
            var parseResult = sp.ParseCSP(inputPath);
            Problem<int> csp = new Problem<int>(parseResult.Item1, parseResult.Item2);
            Solution<int> solution = csp.Solve(config);
            SkyscraperWriter sw = new SkyscraperWriter(csp, solution);
            sw.WriteSolution(outputPath);
            Console.WriteLine("Job Done! Path to result: " + outputPath);
            if (sendMail) SendMail("238572@student.pwr.edu.pl", "Job Done!", "Path to result: " + outputPath);
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(outputPath)
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public static void SendMail(string mailTo, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Timeout = 20000;
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("email", "password");

                MailMessage message = new MailMessage();
                message.To.Add(mailTo);
                message.Subject = subject;
                message.From = new MailAddress("email");
                message.Body = body;

                client.Send(message);
                message.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot send mail! " + e.ToString());
            }
        }
    }
}
