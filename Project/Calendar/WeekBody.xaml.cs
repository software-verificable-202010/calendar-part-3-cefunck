﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calendar
{
    /// <summary>
    /// Lógica de interacción para WeekBody.xaml
    /// </summary>
    public partial class WeekBody : UserControl
    {
        #region Constants
        private const int saturdayGridColumnIndex = 6;
        private const int sundayGridColumnIndex = 7;
        #endregion


        #region Fields
        private readonly Brush highlightColor = Brushes.Red;
        private List<WeekColumn> dayColumns = new List<WeekColumn>();
        private List<Appointment> monthAppointmens = new List<Appointment>();
        #endregion


        #region Properties
        #endregion


        #region Methods
        public WeekBody()
        {
            InitializeComponent();

        }

        public void Refresh() 
        {
            GenerateDayColumnElements();
            InsertBodyElements();
            RefreshDayColumnElements();
            HighLightWeekends();
        }

        public void AssignMonthBodyAppointmnts(List<Appointment> appointments)
        {
            monthAppointmens = appointments;
        }

        private void InsertBodyElements()
        {
            WeekBodyGrid.Children.Clear();
            InsertHoursColumnToGrid();
            InsertDayColumnElementsToGrid();
        }

        private void InsertDayColumnElementsToGrid()
        {
            foreach (WeekColumn dayColumnElement in dayColumns)
            {
                WeekBodyGrid.Children.Add(dayColumnElement);
            }
        }

        private void InsertHoursColumnToGrid()
        {
            int hoursColumnIndex = 0;
            WeekHoursColumn weekHoursColumn = new WeekHoursColumn();
            weekHoursColumn.SetValue(Grid.ColumnProperty, hoursColumnIndex);
            WeekBodyGrid.Children.Add(weekHoursColumn);
        }

        private void HighLightWeekends()
        {
            foreach (var child in WeekBodyGrid.Children)
            {
                if (IsDayColumnElement(child))
                {
                    WeekColumn dayElementChild = (child as WeekColumn);
                    int childColumnIndex = (int)dayElementChild.GetValue(Grid.ColumnProperty);
                    if (IsInWeekendColumn(childColumnIndex))
                    {
                        dayElementChild.Foreground = highlightColor;
                    }
                }
            }
        }

        private void RefreshDayColumnElements()
        {
            foreach (WeekColumn dayColumnElement in dayColumns)
            {
                dayColumnElement.AssignDayAppointments(GetDayAppointments(dayColumnElement));
                dayColumnElement.Refresh();
            }
        }

        private void GenerateDayColumnElements()
        {
            dayColumns = new List<WeekColumn>();
            for (int dayNumberInWeek = 1; dayNumberInWeek <= Utilities.DaysInWeek; dayNumberInWeek++)
            {
                DateTime displayedDate = Utilities.DisplayedDate;
                int displayedDateDayOfWeek = Utilities.GetDayNumberInWeek(displayedDate);
                int daysFromDisplayedDate = Utilities.NegativeMultiplier * displayedDateDayOfWeek + dayNumberInWeek;
                DateTime dayColumnDate = displayedDate.AddDays(daysFromDisplayedDate);
                WeekColumn weekColumnElement = new WeekColumn(dayColumnDate, dayNumberInWeek);
                weekColumnElement.SetValue(Grid.ColumnProperty, dayNumberInWeek);
                dayColumns.Add(weekColumnElement);
            }
        }

        private static bool IsDayColumnElement(object child)
        {
            bool isTypeOfWeekColumn = child.GetType() == typeof(WeekColumn);
            return isTypeOfWeekColumn;
        }

        private static bool IsInWeekendColumn(int childColumnIndex)
        {
            bool isInSaturday = childColumnIndex == saturdayGridColumnIndex;
            bool isInSunday = childColumnIndex == sundayGridColumnIndex;
            bool isInWeekend = isInSaturday | isInSunday;

            return isInWeekend;
        }

        private static bool IsAppointmentOfDay(Appointment appointment, WeekColumn dayColumnElement)
        {
            bool haveTheSameDate = appointment.Start.Date == dayColumnElement.Date.Date;
            return haveTheSameDate;
        }

        private List<Appointment> GetDayAppointments(WeekColumn dayElement)
        {
            List<Appointment> dayElementAppointments = new List<Appointment>();
            foreach (Appointment appointment in monthAppointmens)
            {
                bool hasReadPermission = appointment.HasReadPermissions(SessionController.CurrenUser);
                bool isOfTheDayElement = IsAppointmentOfDay(appointment, dayElement);
                if (isOfTheDayElement & hasReadPermission)
                {
                    dayElementAppointments.Add(appointment);
                }
            }
            return dayElementAppointments;
        }

        #endregion
    }
}
