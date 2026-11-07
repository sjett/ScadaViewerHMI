// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Media;
using HMI.Infrastructure.Presentation.Views;
using HMI.Editor.Common;

namespace HMI.Editor.Surface.Transformations
{
	public class SnapManager
	{
		private readonly ControlContainer controlContainer;
		private readonly UIElement decorator;
		private readonly DesignerCanvas designerCanvas;
		public SnapManager(UIElement decorator, ControlContainer controlContainer)
		{
			this.decorator = decorator;
			this.controlContainer = controlContainer;
            designerCanvas = GetDesignerCanvas();
		}
		public void PerformAnchorSnapping(double controlRelativeSnapAnchorX, double controlRelativeSnapAnchorY, ref double horizontalChange, ref double verticalChange, ref double minAbsXSnapDelta, ref double minAbsYSnapDelta, SnapMode snapMode)
		{
			GeneralTransform generalTransform = decorator.TransformToAncestor(controlContainer);
			Point point = generalTransform.Transform(new Point(controlRelativeSnapAnchorX, controlRelativeSnapAnchorY));
			GeneralTransform generalTransform2 = controlContainer.TransformToAncestor(designerCanvas);
			GeneralTransform generalTransform3 = designerCanvas.TransformToDescendant(controlContainer);
			Point point2 = generalTransform2.Transform(point);
			point2.Offset(horizontalChange, verticalChange);
			Point closestSnapPoint = designerCanvas.GetClosestSnapPoint(point2, snapMode);
			Point point3 = generalTransform3.Transform(closestSnapPoint);
			double horizontalSnapDelta = Math.Abs(closestSnapPoint.X - point2.X);
			if (horizontalSnapDelta < designerCanvas.GridSnapThreshold && horizontalSnapDelta < minAbsXSnapDelta)
			{
				horizontalChange = point3.X - point.X;
				minAbsXSnapDelta = horizontalSnapDelta;
			}
			double verticalSnapDelta = Math.Abs(closestSnapPoint.Y - point2.Y);
			if (verticalSnapDelta < designerCanvas.GridSnapThreshold && verticalSnapDelta < minAbsYSnapDelta)
			{
				verticalChange = point3.Y - point.Y;
				minAbsYSnapDelta = verticalSnapDelta;
			}
		}
		private DesignerCanvas GetDesignerCanvas()
		{
			return VisualTreeNavigator.FindAncestor<DesignerCanvas>(controlContainer);
		}
	}
}
