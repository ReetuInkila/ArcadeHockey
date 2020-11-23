using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Fysiikkapeli1 : PhysicsGame
{
    public override void Begin()
    {
        Random rand = new Random();
        int suunta = rand.Next(0, 1);
        Label naytto = new Label();
        naytto.Font = new Font(70);
        naytto.TextColor = Color.Black;
        naytto.X = 0;
        naytto.Y = 0;
        ;

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

}
