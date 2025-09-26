using System.Diagnostics;

namespace Injections
{
    public class OsCommandInjection
    {
        public void RunOsCommand(string command)
        {
            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            var process = Process.Start(command);
        }
        //{/fact}

        public void RunOsCommand2(string command)
        {

            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            var process = Process.Start("constant");
            //{/fact}
        }


        public void RunOsCommandWithArgs(string command, string arguments)
        {
            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            var process = Process.Start(command, arguments);
            //{/fact}
        }

        public void RunOsCommandWithArgs2(string command, string arguments)
        {
            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            var process = Process.Start("constant", "constant");
            //{/fact}
        }


        public void RunOsCommandWithProcessParam(string command)
        {
            Process process = new Process();

            process.StartInfo.FileName = command;

            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            process.Start();
            //{/fact}
        }

        public void RunOsCommandWithProcessParam2(string command)
        {
            Process process = new Process();

            process.StartInfo.FileName = "constant";

            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            process.Start();
            //{/fact}
        }


        public void RunOsCommandAndArgsWithProcessParam(string command, string arguments)
        {
            Process process = new Process();

            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;

            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            process.Start();
            //{/fact}
        }

        public void RunOsCommandAndArgsWithProcessParam2(string command, string arguments)
        {
            Process process = new Process();

            process.StartInfo.FileName = "constant";
            process.StartInfo.Arguments = "constant";

            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            process.Start();
            //{/fact}
        }


        public void RunOsCommandWithStartInfo(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = command
            };

            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            var process = Process.Start(processStartInfo);
            //{/fact}
        }

        public void RunOsCommandWithStartInfo2(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "constant"
            };

            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            var process = Process.Start(processStartInfo);
            //{/fact}
        }


        public void RunConstantAppWithArgs(string args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "constant",
                Arguments = args
            };

            //{fact rule=os-command-injection@v1.0 defects=1}
            // ruleid: os-command-injection
            var process = Process.Start(processStartInfo);
            //{/fact}
        }

        public void RunConstantAppWithArgs2(string args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "constant",
                Arguments = "constant"
            };

            //{fact rule=os-command-injection@v1.0 defects=0}
            // ok: os-command-injection
            var process = Process.Start(processStartInfo);
        }
    }
}
            //{/fact}
