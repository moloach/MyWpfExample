using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// StatelessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StatelessWindow : Window
    {
        public StatelessWindow()
        {
            InitializeComponent();
        }

        


        public enum State
        {
            Open,
            Assigned,
            Deferred,
            Closed
        }


        public enum Trigger
        {
            Assign,
            Defer,
            Close
        }


        class Bug
        {
            State _state = State.Open;
            StateMachine<State, Trigger> _machine;
            StateMachine<State, Trigger>.TriggerWithParameters<string> _assignTrigger;

            string _title;
            string _assignee;

            public Bug(string title)
            {
                _title = title;

                _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);

                _assignTrigger = _machine.SetTriggerParameters<string>(Trigger.Assign);

                _machine.Configure(State.Open).Permit(Trigger.Assign, State.Assigned);
                _machine.Configure(State.Assigned)
                    .OnEntryFrom(_assignTrigger, assignee => _assignee = assignee)
                    .SubstateOf(State.Open)
                    .PermitReentry(Trigger.Assign)
                    .Permit(Trigger.Close, State.Closed)
                    .Permit(Trigger.Defer, State.Deferred);

                _machine.Configure(State.Deferred)
                    .OnEntry(() => _assignee = null)
                    .Permit(Trigger.Assign, State.Assigned);
            }

            public string CurrentState
            {
                get
                {
                    return _machine.State.ToString();
                }
            }

            public string Title
            {
                get
                {
                    return _title;
                }
            }

            public string Assignee
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_assignee))
                    {
                        return "Not Assigned";
                    }

                    return _assignee;
                }
            }


            public void Assign(string assignee)
            {
                _machine.Fire(_assignTrigger, assignee);
            }

            public void Defer()
            {
                _machine.Fire(Trigger.Defer);
            }

            public void Close()
            {
                _machine.Fire(Trigger.Close);
            }
        }


        private void Test(object sender, RoutedEventArgs e)
        {
             Bug bug = new Bug("Hello World!");

            Console.WriteLine($"Current State: {bug.CurrentState}");
            Console.WriteLine($"Current Assignee: {bug.Assignee}");

            bug.Assign("Lamond Lu");

            Console.WriteLine($"Current State: {bug.CurrentState}");
            Console.WriteLine($"Current Assignee: {bug.Assignee}");

            bug.Defer();

            Console.WriteLine($"Current State: {bug.CurrentState}");
            Console.WriteLine($"Current Assignee: {bug.Assignee}");

            bug.Assign("Lu Nan");

            Console.WriteLine($"Current State: {bug.CurrentState}");
            Console.WriteLine($"Current Assignee: {bug.Assignee}");

            bug.Close();

            Console.WriteLine($"Current State: {bug.CurrentState}");
        }
    }
}
