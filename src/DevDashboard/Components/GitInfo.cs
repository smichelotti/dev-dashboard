using DevDashboard.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace DevDashboard.Components
{
    public class GitInfo : DashComponent
    {
        // TODO: get/set state to file
        // TODO: modal dialog to select repo location
        // TODO: refresh git log when branch changes (?)
        // TODO: general refresh on terminal focus (?)

        private string gitDir = @"c:\dev\ato-toolkit"; // sets initial (TODO: remove hard-coding)
        private Button selectBtn;
        private Label gitPath;
        private FrameView branches;
        private Label branchesList;
        private FrameView log;
        private Label gitLog;

        public GitInfo() => this.Name = $"Git";

        public override void Setup()
        {
            this.selectBtn = new Button(x: 0, y: 0, text: "Select Repo")
            {
                Clicked = () =>
                {
                    var dialog = new Dialog("Specify repo directory");
                    var txtDir = new TextField()
                    {
                        X = 1,
                        Y = 1,
                        Width = Dim.Percent(50)
                    };
                    var ok = new Button("OK")
                    {
                        Clicked = () =>
                        {
                            this.gitDir = txtDir.Text.ToString();
                            Application.RequestStop();
                            //this.RefreshGitLog();
                        }
                    };
                    dialog.Add(txtDir);
                    dialog.AddButton(ok);
                    Application.Run(dialog);
                    this.RefreshGitLog();
                }
            };

            this.gitPath = new Label($"({this.gitDir})")
            {
                X = Pos.Right(this.selectBtn) + 3,
                Y = 0,
                Width = Dim.Fill()
            };

            this.branches = new FrameView("Branches")
            {
                Y = 2,
                Height = Dim.Fill(),
                Width = Dim.Percent(25)
            };

            this.branchesList = new Label(this.GetGitOutput("branch"))
            {
                Width = Dim.Fill(),
                ColorScheme = ColorSchemes.Get(Color.BrighCyan)
            };
            this.branches.Add(this.branchesList);


            this.log = new FrameView("Log")
            {
                X = Pos.Right(this.branches),
                Y = 2,
                Height = Dim.Fill(),
                Width = Dim.Percent(75)
            };

            // Explicitly set x & y in order to turn off word wrap
            this.gitLog = new Label(x: 0, y: 0, this.GetGitOutput("l -n 10"))
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = ColorSchemes.Get(Color.BrightYellow)
            };
            this.log.Add(this.gitLog);


            this.Frame.Add(this.selectBtn, this.gitPath, this.branches, this.log);
        }

        private void RefreshGitLog()
        {
            this.gitPath.Text = $"({this.gitDir})";
            this.branchesList.Text = this.GetGitOutput("branch");
            this.gitLog.Text = this.GetGitOutput("log --graph --decorate --pretty=oneline --abbrev-commit -n 10");
        }

        private string GetGitOutput(string gitArgs)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = this.gitDir,
                    FileName = "git",
                    Arguments = gitArgs,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            return proc.StandardOutput.ReadToEnd();
        }
    }
}
