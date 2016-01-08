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
using NiceLife;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Add : Page
    {
        private Plan p_ini;
        private Call c_ini;
        private string title;
        private string description;
        private long PlanId;
        private string audioSrc;
        private string isSilent;
        private long colorId=-1;
        private long remindId;
        private DateTime remindDate;
        private string remindType;
        private DateTime beginDate;
        private DateTime endDate;

        private int isRemind;
        private long last;
        private int loop;
        private List<ColorLable> color;
        
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
            BeginDate.Date = dateTime.Date;
            BeginTime.Time = dateTime.TimeOfDay;
            EndTime.Time = dateTime.AddHours(1).TimeOfDay;
            ColorLableHelper clp = ColorLableHelper.GetHelper();
            color = clp.SelectlistItems();
            for (int i = 0; i < color.Count(); i++)
            {
                comboBox_color.Items.Add(color.ElementAt(i).Color);
            }
          //  comboBox_color.SelectedIndex = 0;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            p_ini = (Plan)e.Parameter;
            if (p_ini != null)
            {
                PlanId = p_ini.Id;
                textBox_title.Text = p_ini.Title;
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                //   comboBox_color.SelectedValue = clp.SelectSingleItemById(p_ini.ColorId);
                textBox_dsp.Text = p_ini.Description;
                BeginDate.Date = p_ini.BeginDate.Date;
                BeginTime.Time = p_ini.BeginDate.TimeOfDay;
                EndTime.Time = p_ini.EndDate.TimeOfDay;
                textBox_last.Text = p_ini.Last.ToString();
                if (p_ini.Loop == 1) checkBox_year.IsChecked = true;
                if (p_ini.Loop == 2) checkBox_month.IsChecked = true;
                if (p_ini.Loop == 3) checkBox_week.IsChecked = true;
                if (p_ini.Loop == 4) checkBox_day.IsChecked = true;
                if (p_ini.IsRemind == 0) radioButton_N.IsChecked = true;
                else
                {
                    radioButton_Y.IsChecked = true;
                    CallHelper ch = CallHelper.GetHelper();
                    c_ini = ch.SelectSingleItemById(p_ini.Id);
                    if (c_ini != null)
                    {
                        RemindDate.Date = c_ini.Date.Date;
                        RemindTime.Time = c_ini.Date.TimeOfDay;
                        comboBox_remind.SelectedValue = c_ini.Type;
                    }
                }
            }

        }
        private void save_Click(object sender, RoutedEventArgs e)
        {

            title = textBox_title.Text;
            colorId = comboBox_color.SelectedIndex;
            description = textBox_dsp.Text;
            beginDate = BeginDate.Date.Value.Date;
            beginDate = beginDate.Add(BeginTime.Time);
            endDate = BeginDate.Date.Value.Date;
            endDate = BeginDate.Date.Value.Date.Add(BeginTime.Time);
            audioSrc =comboBox_remind.SelectedValue.ToString();
           
            if(comboBox_color.SelectedIndex!=-1)
            colorId = color.ElementAt(comboBox_color.SelectedIndex).Id;
            if (textBox_last.Text != "")
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
            if (radioButton_Y.IsChecked.Value)
            {
                isRemind = 1;
                remindDate = RemindDate.Date.Value.Date;
                remindDate.Add(RemindTime.Time);
                if (comboBox_remind.SelectedValue != null)
                {
                    remindType = comboBox_remind.SelectedValue.ToString();
                }

            }
            else
            {
                isRemind = 0;
            }
            if (p_ini == null)
            {
                if (textBox_title.Text == "")
                {
                    ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                    XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                    XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode("the title is null!"));
                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);


                }
                else {
                   
                    Plan plan = new Plan();
       
                    plan.Title = title;
                    plan.ColorId = colorId;
                    plan.IsRemind = isRemind;
                    plan.Description = description;
                    plan.BeginDate = beginDate;
                    plan.EndDate = endDate;
                    plan.Last = last;
                    plan.Loop = loop;
                    PlanHelper ph = PlanHelper.GetHelper();
                    ph.InsertSingleItem(plan);
                    PlanId = ph.GetMaxId();
                    check_remind();
                    
                    DateTime bd = beginDate, ed = endDate;
                    if (loop == 0)
                    {
                        if (last != 0)
                        {
                            for (int i = 0; i < last; i++)
                            {

                                plan.BeginDate = bd.AddDays(1);
                                plan.EndDate = ed.AddDays(1);
                                bd = bd.AddDays(1);
                                remindDate = remindDate.AddDays(1);
                                ed = ed.AddDays(1);
                                ph.InsertSingleItem(plan);
                                PlanId = ph.GetMaxId();
                                check_remind();

                            }
                        }

                    }
                    if (loop != 0)
                    {
                        if (loop == 3)
                        {
                            while (bd.Year < DateTime.Today.Year + 2)
                            {
                                if (last != 0)
                                {
                                    for (int i = 0; i < last; i++)
                                    {

                                        plan.BeginDate = bd.AddDays(1);
                                        plan.EndDate = ed.AddDays(1);
                                        bd = bd.AddDays(1);
                                        ed = ed.AddDays(1);
                                        remindDate = remindDate.AddDays(1);
                                        ph.InsertSingleItem(plan);
                                        PlanId = ph.GetMaxId();
                                        check_remind();

                                    }
                                }

                                bd = bd.AddDays(7 - last);
                                ed = ed.AddDays(7 - last);
                                remindDate = remindDate.AddDays(7 - last);
                                plan.BeginDate = bd;
                                plan.EndDate = ed;
                                ph.InsertSingleItem(plan);
                                PlanId = ph.GetMaxId();
                                check_remind();
                            }
                        }
                        if (loop == 2)
                        {
                            while (bd.Year < DateTime.Today.Year + 2)
                            {
                                if (last != 0)
                                {
                                    for (int i = 0; i < last; i++)
                                    {

                                        plan.BeginDate = bd.AddDays(1);
                                        plan.EndDate = ed.AddDays(1);
                                        bd = bd.AddDays(1);
                                        ed = ed.AddDays(1);
                                        remindDate = remindDate.AddDays(1);
                                        ph.InsertSingleItem(plan);
                                        PlanId = ph.GetMaxId();
                                        check_remind();

                                    }
                                }
                                bd = bd.AddDays(-last);
                                ed = ed.AddDays(-last);
                                remindDate = remindDate.AddDays(-last);
                                bd = bd.AddMonths(1);
                                ed = ed.AddMonths(1);
                                remindDate = remindDate.AddMonths(1);
                                plan.BeginDate = bd;
                                plan.EndDate = ed;
                                ph.InsertSingleItem(plan);
                                PlanId = ph.GetMaxId();
                                check_remind();
                            }

                        }
                        if (loop == 1)
                        {
                            while (bd.Year < DateTime.Today.Year + 100)
                            {
                                if (last != 0)
                                {
                                    for (int i = 0; i < last; i++)
                                    {
                                        ph.InsertSingleItem(plan);
                                        plan.BeginDate = bd.AddDays(1);
                                        plan.EndDate = ed.AddDays(1);
                                        remindDate = remindDate.AddDays(1);
                                        bd = bd.AddDays(1);
                                        ed = ed.AddDays(1);
                                        ph.InsertSingleItem(plan);
                                        PlanId = ph.GetMaxId();
                                        check_remind();

                                    }
                                }
                                bd = bd.AddDays(-last);
                                ed = ed.AddDays(-last);
                                remindDate = remindDate.AddDays(1);
                                bd = bd.AddYears(1);
                                ed = ed.AddYears(1);
                                remindDate = remindDate.AddYears(1);
                                plan.BeginDate = bd;
                                plan.EndDate = ed;
                                ph.InsertSingleItem(plan);
                                PlanId = ph.GetMaxId();
                                check_remind();
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
            else
            {

                p_ini.Title = title;
                p_ini.ColorId = colorId;
                p_ini.Description = description;
                p_ini.BeginDate = beginDate;
                p_ini.EndDate = endDate;
                p_ini.Last = last;
                p_ini.Loop = loop;
                p_ini.IsRemind = isRemind;
                if (isRemind == 1)
                {
                    if (c_ini != null)
                    {
                        c_ini.Date = remindDate;
                        c_ini.Date = c_ini.Date.Add(RemindTime.Time);
                        c_ini.Type = remindType;
                        CallHelper cp = CallHelper.GetHelper();
                        cp.UpdateSingleItem(c_ini);
                        
                    }
                    else
                    {
                        check_remind();
                    }
                }
                else
                {
                    if (c_ini != null)
                    {
                        CallHelper cp = CallHelper.GetHelper();
                        cp.DeleteSingleItemById(c_ini.Id);
                    }
                }
               
               
                PlanHelper ph = PlanHelper.GetHelper();
                ph.UpdateSingleItem(p_ini);
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("Setting sucessed!"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
                this.Visibility = Visibility.Collapsed;
            }

           
    }

        public void check_remind()
        {
            Call remind = new Call();
            if (isRemind == 1)
            {
                remind.PlanId = PlanId;
                remind.Date = RemindDate.Date.Value.Date;

                remind.Date = remind.Date.Add(RemindTime.Time);
                remind.Type = remindType;
                remind.State = 1;
                if (RemindDate.Date.Value.Date == DateTime.Today)
                {
                    try
                    {

                        XmlDocument xdoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

                        var txtnodes = xdoc.GetElementsByTagName("text");
                        txtnodes[0].InnerText = title + ":" + description;

                        ScheduledToastNotification toast3 = new ScheduledToastNotification(xdoc, remind.Date);
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast3);
                    }
                    catch (Exception ex)
                    {


                    }
                }
                CallHelper ch = CallHelper.GetHelper();
                ch.InsertSingleItem(remind);
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
                last_ini();

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
                last_ini();
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
                last_ini();
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
                last_ini();
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
                last_ini();
            }
        }

        private void BeginDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            beginDate = BeginDate.Date.Value.Date;

           

            endDate = BeginDate.Date.Value.Date;

            
        }

       public void last_ini()
        {

            if (textBox_last.Text != "")
            {
                if (Convert.ToInt16(textBox_last.Text) > 0)
                {
                    checkBox_day.IsEnabled = false;
                }
                if (Convert.ToInt16(textBox_last.Text) > 7)
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
        }

        private void textBox_last_TextChanged(object sender, TextChangedEventArgs e)
        {

            last_ini();
        }

        private void radioButton_Y_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton_Y.IsChecked.Value)
            {

                RemindDate.IsEnabled = true;
                RemindDate.Date = DateTime.Now.Date;
                RemindTime.IsEnabled = true;
                RemindTime.Time = DateTime.Now.TimeOfDay;
                comboBox_remind.IsEnabled = true;
                radioButton_N.IsChecked = false;
            }
           
        }

        private void BeginTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            beginDate = BeginDate.Date.Value.Date;
            beginDate = beginDate.Add(BeginTime.Time);
            EndTime.Time = BeginTime.Time;
        }

        private void EndTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            endDate= BeginDate.Date.Value.Date.Add(EndTime.Time);
        }

        private void comboBox_color_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {

        }
    }
    
}
