// SPDX-License-Identifier: GPL-3.0-only
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace HMI.Editor.Common;

public class BrushJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Brush).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Brush brush = (Brush)value;

        JObject obj = SerializeBrush(brush);
        obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        return DeserializeBrush(obj);
    }

    public static JObject SerializeBrush(Brush brush)
    {
        if (brush is SolidColorBrush solid)
        {
            return new JObject
            {
                ["BrushType"] = "SolidColorBrush",
                ["Color"] = solid.Color.ToString()
            };
        }

        if (brush is LinearGradientBrush linear)
            return SerializeLinearGradientBrush(linear);

        if (brush is RadialGradientBrush radial)
            return SerializeRadialGradientBrush(radial);

        return new JObject
        {
            ["BrushType"] = "Unknown"
        };
    }
    private static JObject SerializeLinearGradientBrush(LinearGradientBrush brush)
    {
        return new JObject
        {
            ["BrushType"] = "LinearGradientBrush",
            ["StartPoint"] = $"{brush.StartPoint.X},{brush.StartPoint.Y}",
            ["EndPoint"] = $"{brush.EndPoint.X},{brush.EndPoint.Y}",
            ["GradientStops"] = new JArray(
                brush.GradientStops.Select(gs => new JObject
                {
                    ["Color"] = gs.Color.ToString(),
                    ["Offset"] = gs.Offset
                })
            )
        };
    }
    private static JObject SerializeRadialGradientBrush(RadialGradientBrush brush)
    {
        return new JObject
        {
            ["BrushType"] = "RadialGradientBrush",
            ["Center"] = $"{brush.Center.X},{brush.Center.Y}",
            ["RadiusX"] = brush.RadiusX,
            ["RadiusY"] = brush.RadiusY,
            ["GradientStops"] = new JArray(
                brush.GradientStops.Select(gs => new JObject
                {
                    ["Color"] = gs.Color.ToString(),
                    ["Offset"] = gs.Offset
                })
            )
        };
    }
    public static Brush DeserializeBrush(JObject json)
    {
        string type = json["BrushType"]?.ToString();

        switch (type)
        {
            case "SolidColorBrush":
                return new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(json["Color"].ToString()));

            case "LinearGradientBrush":
                return DeserializeLinearGradientBrush(json);

            case "RadialGradientBrush":
                return DeserializeRadialGradientBrush(json);
        }

        return Brushes.Transparent;
    }
    private static Brush DeserializeLinearGradientBrush(JObject json)
    {
        var brush = new LinearGradientBrush();

        var sp = json["StartPoint"].ToString().Split(',');
        brush.StartPoint = new Point(double.Parse(sp[0]), double.Parse(sp[1]));

        var ep = json["EndPoint"].ToString().Split(',');
        brush.EndPoint = new Point(double.Parse(ep[0]), double.Parse(ep[1]));

        foreach (var gs in (JArray)json["GradientStops"])
        {
            brush.GradientStops.Add(new GradientStop
            {
                Color = (Color)ColorConverter.ConvertFromString(gs["Color"].ToString()),
                Offset = (double)gs["Offset"]
            });
        }

        return brush;
    }
    private static Brush DeserializeRadialGradientBrush(JObject json)
    {
        var brush = new RadialGradientBrush();

        var center = json["Center"].ToString().Split(',');
        brush.Center = new Point(double.Parse(center[0]), double.Parse(center[1]));

        brush.RadiusX = (double)json["RadiusX"];
        brush.RadiusY = (double)json["RadiusY"];

        foreach (var gs in (JArray)json["GradientStops"])
        {
            brush.GradientStops.Add(new GradientStop
            {
                Color = (Color)ColorConverter.ConvertFromString(gs["Color"].ToString()),
                Offset = (double)gs["Offset"]
            });
        }

        return brush;
    }

}

