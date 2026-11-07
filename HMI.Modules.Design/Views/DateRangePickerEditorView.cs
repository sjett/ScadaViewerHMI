// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Views;
using HMI.Infrastructure.Presentation;
using HMI.Modules.Design.Resources;
using HMI.Modules.Design.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using Range = HMI.Infrastructure.Presentation.Range;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(RadDropDownButton))]
	public class DateRangePickerEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadDropDownButton dateRangeDropDownButton;
		public static readonly DependencyProperty SelectedDateRangeProperty;
		public static readonly DependencyProperty StartDateProperty;
		public static readonly DependencyProperty EndDateProperty;
		public static readonly DependencyProperty RangeProperty;
		public static readonly DependencyProperty NumberProperty;
		
		private static Func<DateRange, DateRangeViewModel> dateRangeViewModelFactory;
		public DateRange SelectedDateRange
		{
			get
			{
				return (DateRange)base.GetValue(DateRangePickerEditorView.SelectedDateRangeProperty);
			}
			set
			{
				base.SetValue(DateRangePickerEditorView.SelectedDateRangeProperty, value);
			}
		}
		public DateTime StartDate
		{
			get
			{
				return (DateTime)base.GetValue(DateRangePickerEditorView.StartDateProperty);
			}
			set
			{
				base.SetValue(DateRangePickerEditorView.StartDateProperty, value);
			}
		}
		public DateTime? EndDate
		{
			get
			{
				return (DateTime?)base.GetValue(DateRangePickerEditorView.EndDateProperty);
			}
			set
			{
				base.SetValue(DateRangePickerEditorView.EndDateProperty, value);
			}
		}
		public Range Range
		{
			get
			{
				return (Range)base.GetValue(DateRangePickerEditorView.RangeProperty);
			}
			set
			{
				base.SetValue(DateRangePickerEditorView.RangeProperty, value);
			}
		}
		public int Number
		{
			get
			{
				return (int)base.GetValue(DateRangePickerEditorView.NumberProperty);
			}
			set
			{
				base.SetValue(DateRangePickerEditorView.NumberProperty, value);
			}
		}
		public List<DateRangeViewModel> DateRanges
		{
			get;
			private set;
		}
		static DateRangePickerEditorView()
		{
			DateRangePickerEditorView.SelectedDateRangeProperty = DependencyProperty.Register("SelectedDateRange", typeof(DateRange), typeof(DateRangePickerEditorView), new PropertyMetadata(DateRange.None, new PropertyChangedCallback(OnDateRangeSelectionChanged)));
			DateRangePickerEditorView.StartDateProperty = DependencyProperty.Register("StartDate", typeof(DateTime), typeof(DateRangePickerEditorView), new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(OnDateRangeSelectionChanged)));
			DateRangePickerEditorView.EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime?), typeof(DateRangePickerEditorView), new PropertyMetadata(null, new PropertyChangedCallback(OnDateRangeSelectionChanged)));
			DateRangePickerEditorView.RangeProperty = DependencyProperty.Register("Range", typeof(Range), typeof(DateRangePickerEditorView), new PropertyMetadata(default(Range)));
			DateRangePickerEditorView.NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(DateRangePickerEditorView), new PropertyMetadata(0, new PropertyChangedCallback(OnDateRangeSelectionChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DateRangePickerEditorView), new FrameworkPropertyMetadata(typeof(DateRangePickerEditorView)));
		}
        static void OnDateRangeSelectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            DateRangePickerEditorView editor = (DateRangePickerEditorView)dependencyObject;
            UpdateSelectedRange(editor);
        }
        static void UpdateSelectedRange(DateRangePickerEditorView editor)
        {
            switch (editor.SelectedDateRange)
            {
                case DateRange.None:
                    editor.Range = Range.None;
                    return;
                case DateRange.Day:
                case DateRange.Week:
                case DateRange.Month:
                case DateRange.Year:
                    editor.Range = new Range(editor.SelectedDateRange, editor.StartDate, null, 0);
                    return;
                case DateRange.Range:
                    editor.Range = new Range(editor.SelectedDateRange, editor.StartDate, editor.EndDate, 0);
                    return;
                case DateRange.Yesterday:
                    editor.Range = new Range(DateRange.Yesterday);
                    return;
                case DateRange.CurrentDay:
                    editor.Range = new Range(DateRange.CurrentDay);
                    return;
                case DateRange.LastNDays:
                    editor.Range = new Range(DateRange.LastNDays, editor.Number);
                    return;
                case DateRange.LastNHours:
                    editor.Range = new Range(DateRange.LastNHours, editor.Number);
                    return;
                case DateRange.LastNMinutes:
                    editor.Range = new Range(DateRange.LastNMinutes, editor.Number);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public DateRangePickerEditorView()
		{
			IEnumerable<DateRange> dateRanges = Enum.GetValues(typeof(DateRange)).Cast<DateRange>();
			if (DateRangePickerEditorView.dateRangeViewModelFactory == null)
			{
				DateRangePickerEditorView.dateRangeViewModelFactory = new Func<DateRange, DateRangeViewModel>(DateRangePickerEditorView.CreateDateRangeViewModel);
			}
			this.DateRanges = dateRanges.Select(DateRangePickerEditorView.dateRangeViewModelFactory).ToList<DateRangeViewModel>();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.dateRangeDropDownButton = this.FindTemplateChild<RadDropDownButton>(EditorTemplatePartName);
			if (this.dateRangeDropDownButton == null)
			{
				return;
			}
			base.SetBinding(DateRangePickerEditorView.RangeProperty, base.ValueBinding);
			DateRange dateRange = this.Range.dateRange;
			DateTime startDate = this.Range.startDate;
			DateTime? endDate = this.Range.endDate;
			int number = this.Range.number;
			this.SelectedDateRange = dateRange;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.Number = number;
		}
		
		private static DateRangeViewModel CreateDateRangeViewModel(DateRange dateRange)
		{
			return new DateRangeViewModel
			{
				Range = dateRange,
				Text = DateRangeTexts.ResourceManager.GetString("DateRange_" + dateRange)
			};
		}
	}
}
