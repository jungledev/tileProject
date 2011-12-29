using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;

namespace TileProject
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void btnAddTile_Click(object sender, RoutedEventArgs e)
        {
            //add tile
            //start background agent 
            PeriodicTask periodicTask = new PeriodicTask("PeriodicAgent");

            periodicTask.Description = "Live Tile Update - TileProject";
            periodicTask.ExpirationTime = System.DateTime.Now.AddDays(1);

            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove("PeriodicAgent");
            }

            ScheduledActionService.Add(periodicTask);

            ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(10));
        }
    }
}