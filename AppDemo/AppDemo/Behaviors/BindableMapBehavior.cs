using AppDemo.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppDemo.Behaviors
{
    public class BindableMapBehavior : BindableBehavior<Map>
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<BindableMapBehavior, IEnumerable<PinRequest>>(p => p.ItemsSource, null, BindingMode.Default, null, ItemsSourceChanged);

        public IEnumerable<PinRequest> ItemsSource
        {
            get { return (IEnumerable<PinRequest>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }

        }
        static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        {
            var behavior = bindable as BindableMapBehavior;
            behavior?.SynchronizePins();
        }
        void SynchronizePins()
        {
            var map = LinkedElement;

            for (int pinIndex = map.Pins.Count - 1; pinIndex >= 0; pinIndex--)
            {
                map.Pins[pinIndex].Clicked -= ClickedPinMapToCommand;

                map.Pins.RemoveAt(pinIndex);
            }

            var pins = ItemsSource.Select(source =>
            {
                var pin = new Pin
                {
                    Label = source.placa,
                    Address = "Su tiempo termina a las " + source.HoraFin.ToLocalTime().Hour + ":" + source.HoraFin.ToLocalTime().Minute,
                    Type = PinType.Place,
                    Position = new Position(source.Latitud, source.Longitud),
                };

                pin.Clicked += ClickedPinMapToCommand;

                return pin;
            }).ToArray();

            foreach (var pin in pins)

                map.Pins.Add(pin);
        }

        void ClickedPinMapToCommand(object sender, EventArgs eventArgs)
        {
            var pin = sender as Pin;

            if (pin == null) return;

            var bindableLocation = ItemsSource.FirstOrDefault(x => x.placa == pin.Label);
        }
    }
}