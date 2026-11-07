// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Input;

namespace HMI.Infrastructure
{
    public static class DesignerCanvasCommands
    {
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignMiddle = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignCenter = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand RotateLeft = new RoutedCommand();
        public static RoutedCommand RotateRight = new RoutedCommand();
        public static RoutedCommand MakeSameWidth = new RoutedCommand();
        public static RoutedCommand MakeSameHeight = new RoutedCommand();
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand SetSelectAction = new RoutedCommand();
        public static RoutedCommand SetDrawLineAction = new RoutedCommand();
        public static RoutedCommand SetDrawRectangleAction = new RoutedCommand();
        public static RoutedCommand SetDrawEllipseAction = new RoutedCommand();
        public static RoutedCommand SetDrawPolygonAction = new RoutedCommand();
        public static RoutedCommand SetDrawTextAction = new RoutedCommand();
        public static RoutedCommand ShowGrid = new RoutedCommand();
        public static RoutedCommand ShowGuideLines = new RoutedCommand();
        public static RoutedCommand SnapToGridLines = new RoutedCommand();
        public static RoutedCommand SnapToGuideLines = new RoutedCommand();
    }
}
