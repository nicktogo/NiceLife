using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NiceLife.Schedule.db;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Globalization;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Add : Page
    {

        private string title;
        private string description;
        private long id;

        private long colorId;
        private long remindId;
        private DateTime remindDate;
        private string remindType;
        private DateTime beginDate;
        private DateTime endDate;

        private int isRemind;
        private long last;
        private int loop;
        
        public Add()
        {
            this.InitializeComponent();
            
            ini();
        }
        public void ini()
        {
            textBox_dsp.Text = "";
            textBox_last.Text = "";
            textBox_title.Text = "";
            DateTime dateTime = DateTime.Now;
            String dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            String today = string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            radioButton_N.IsChecked = true;
            RemindDate.IsEnabled = false;
            RemindTime.IsEnabled = false;
            comboBox_remind.IsEnabled = false;
            BeginDate.Date = dateTime;
            EndDate.Date = dateTime;
        }
        
        private void save_Click(object sender, RoutedEventArgs e)
        {
            
            
           
            if (textBox_title.Text == "")
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("the title is null!"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
               
              /*  if (!radioButton_Y.IsChecked.Value&&!radioButton_N.IsChecked.Value)
                {
                    ToastTemplateType toastTemplate2 = ToastTemplateType.ToastImageAndText01;
                    XmlDocument toastXml2 = ToastNotificationManager.GetTemplateContent(toastTemplate2);
                    XmlNodeList toastTextElements2 = toastXml2.GetElementsByTagName("Remind");
                    toastTextElements2[0].AppendChild(toastXml2.CreateTextNode("Do you want to be reminded?"));
                    ToastNotification toast2 = new ToastNotification(toastXml2);
                    ToastNotificationManager.CreateToastNotifier().Show(toast2);
                }*/
               
            }
            else {
                if (radioButton_Y.IsChecked.Value)
                {
                    isRemind = 1;
                    remindDate = RemindDate.Date.Value.UtcDateTime;
                    remindType = comboBox_remind.SelectedValue.ToString();

                }
                else
                {
                    isRemind = 0;
                }
                Plan plan = new Plan();
                title = textBox_title.Text;
                colorId = comboBox_color.SelectedIndex;
                description = textBox_dsp.Text;
                
                
                if(textBox_last.Text!="")
                    last = int.Parse(textBox_last.Text);
               
                if (checkBox_year.IsChecked.Value)
                {
                    loop = 1;
                }
                else
                {
                    if (checkBox_month.IsChecked.Value)
                    {
                        loop = 2;
                    }
                    else
                    {
                        if (checkBox_week.IsChecked.Value)
                        {
                            loop = 3;
                        }
                        else
                        {
                            if (checkBox_day.IsChecked.Value)
                            {
                                loop = 4;
                            }
                            else
                            {
                                loop = 0;
                            }
                        }
                    }
                }
                Call remind = new Call();
                if (isRemind == 1)
                {
                    remind.Date = remindDate;
                    remind.Type = remindType;
                    CallHelper ch = CallHelper.GetHelper();
                    remindId=ch.InsertSingleItem(remind);
                }

                plan.Title = title;
                plan.ColorId = colorId;
                plan.RemindId = remindId;
                plan.IsRemind = isRemind;
                plan.Description = description;
                plan.BeginDate = beginDate;
                plan.EndDate = endDate;
                plan.Last = last;
                plan.Loop = loop;
                PlanHelper ph = PlanHelper.GetHelper();
                ph.InsertSingleItem(plan);

                DateTime bd = beginDate,ed= endDate;
                if (loop != 0)
                {
                    if (loop == 2)
                    {
                        while (bd.Year < DateTime.Today.Year + 100)
                        {
                            if (last != 0)
                            {
                                for (int i = 0; i < last; i++)
                                {
                                    plan.BeginDate = bd.AddDays(1);
                                    plan.EndDate = ed.AddDays(1);
                                    bd = bd.AddDays(1);
                                    ed = ed.AddDays(1);

                                    ph.InsertSingleItem(plan);

                                }
                            }
                            bd.AddDays(7);
                            ed.AddDays(7);
                        }
                    }
                    if (loop == 3)
                    {
                        while (bd.Year < DateTime.Today.Year + 100)
                        {
                            if (last != 0)
                            {
                                for (int i = 0; i < last; i++)
                                {
                                    plan.BeginDate = bd.AddDays(1);
                                    plan.EndDate = ed.AddDays(1);
                                    bd = bd.AddDays(1);
                                    ed = ed.AddDays(1);

                                    ph.InsertSingleItem(plan);

                                }
                            }
                            bd.AddMonths(1);
                            ed.AddMonths(1);
                        }

                    }
                    if (loop == 4)
                    {
                        while (bd.Year < DateTime.Today.Year + 100)
                        {
                            if (last != 0)
                            {
                                for (int i = 0; i < last; i++)
                                {
                                    plan.BeginDate = bd.AddDays(1);
                                    plan.EndDate = ed.AddDays(1);
                                    bd = bd.AddDays(1);
                                    ed = ed.AddDays(1);

                                    ph.InsertSingleItem(plan);

                                }
                            }
                            bd.AddYears(1);
                            ed.AddYears(1);
                        }
                    }
                }
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("Setting sucessed!"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
                ini();
            }


           
    }

        

        private void radioButton_N_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton_N.IsChecked.Value)
            {
                RemindDate.IsEnabled = false;
                RemindTime.IsEnabled = false;
                comboBox_remind.IsEnabled = false;
                radioButton_Y.IsChecked = false;
            }
            else
            {
               
                RemindDate.IsEnabled = true;
                RemindTime.IsEnabled = true;
                comboBox_remind.IsEnabled = true;
            }
        }

        private void checkBox_week_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_week.IsChecked.Value)
            {
                checkBox_month.IsEnabled = false;
                checkBox_day.IsEnabled = false;
                checkBox_year.IsEnabled = false;
            }
            else
            {
                checkBox_month.IsEnabled = true;
                checkBox_day.IsEnabled = true;
                checkBox_year.IsEnabled = true;
            }
        }

        private void checkBox_day_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_day.IsChecked.Value)
            {
                checkBox_month.IsEnabled = false;
                checkBox_week.IsEnabled = false;
                checkBox_year.IsEnabled = false;
            }
            else
            {
                checkBox_month.IsEnabled = true;
                checkBox_week.IsEnabled = true;
                checkBox_year.IsEnabled = true;
            }
        }

        private void checkBox_month_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_month.IsChecked.Value)
            {
                checkBox_day.IsEnabled = false;
                checkBox_week.IsEnabled = false;
                checkBox_year.IsEnabled = false;
            }
            else
            {
                checkBox_day.IsEnabled = true;
                checkBox_week.IsEnabled = true;
                checkBox_year.IsEnabled = true;
            }
        }

        private void checkBox_year_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_year.IsChecked.Value)
            {
                checkBox_day.IsEnabled = false;
                checkBox_week.IsEnabled = false;
                checkBox_month.IsEnabled = false;
            }
            else
            {
                checkBox_day.IsEnabled = true;
                checkBox_week.IsEnabled = true;
                checkBox_month.IsEnabled = true;
            }
        }

        private void BeginDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            beginDate = BeginDate.Date.Value.UtcDateTime;

            beginDate.Add(BeginTime.Time);
        }

        private void EndDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            endDate = EndDate.Date.Value.UtcDateTime;
            endDate.Add(EndTime.Time);
            if (endDate.Year != beginDate.Year || endDate.Month != beginDate.Month || endDate.Day != beginDate.Day)
            {
                textBox_last.IsEnabled = false;
                last = 0;
            }
        }

        private void textBox_last_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt16(textBox_last.Text) > 0)
            {
                checkBox_day.IsEnabled = false;
            }
            if (Convert.ToInt16(textBox_last.Text)>7)
            {
                checkBox_week.IsEnabled = false;
            }
            if (Convert.ToInt16(textBox_last.Text) > 31)
            {
                checkBox_month.IsEnabled = false;
            }
            if (Convert.ToInt16(textBox_last.Text) > 365)
            {
                checkBox_year.IsEnabled = false;
            }


        }

        private void radioButton_Y_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton_Y.IsChecked.Value)
            {

                RemindDate.IsEnabled = true;
                RemindTime.IsEnabled = true;
                comboBox_remind.IsEnabled = true;
                radioButton_N.IsChecked = false;
            }
           
        }
    }
    
}
