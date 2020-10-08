using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class ArcadeHockey : PhysicsGame
{
    private Vector nopeusYlos = new Vector(0, 200);
    private Vector nopeusAlas = new Vector(0, -200);
    private PhysicsObject pallo;
    private PhysicsObject maila1;
    private PhysicsObject maila2;

    public override void Begin()
    {
        LuoKentta();
        LuoAloitusValikko();
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void LuoAloitusValikko()
    {
        MultiSelectWindow alkuValikko = new MultiSelectWindow("SpaceHockey", 
            "Aloita peli", "Lopeta");
        alkuValikko.AddItemHandler(0, ValitseKenttä);
        alkuValikko.AddItemHandler(1, Exit);
        Add(alkuValikko);
    }
    
    private void ValitseKenttä()
    {
        MultiSelectWindow kentänValinta = new MultiSelectWindow("Valitse Kenttä",
            "Tyhjä kenttä", "Piikkejä", "Palloja", "Palaa");
        kentänValinta.AddItemHandler(0, ValitseMaila);
        kentänValinta.AddItemHandler(1, LuoEsteet1);
        kentänValinta.AddItemHandler(1, ValitseMaila);
        kentänValinta.AddItemHandler(2, LuoEsteet2);
        kentänValinta.AddItemHandler(2, ValitseMaila);
        kentänValinta.AddItemHandler(3, LuoAloitusValikko);
        Add(kentänValinta);
    }
    private void ValitseMaila()
    {
        MultiSelectWindow mailanValinta = new MultiSelectWindow("Valitse Mailat",
            "Suora", "Pyöreä", "Palaa");
        mailanValinta.AddItemHandler(0, LuoMailatSuora);
        mailanValinta.AddItemHandler(0, ValitseTaso);
        mailanValinta.AddItemHandler(1, LuoMailatPyöreä);
        mailanValinta.AddItemHandler(1, ValitseTaso);
        mailanValinta.AddItemHandler(2, ValitseKenttä);
        Add(mailanValinta);
    }

    private void ValitseTaso()
    {
        MultiSelectWindow vaikeustasonValinta = new MultiSelectWindow("Vaikeusaste",
            "Taso 1", "Taso 2", "Taso 3", "Palaa");
        vaikeustasonValinta.AddItemHandler(0, AloitaPeli1);
        vaikeustasonValinta.AddItemHandler(1, AloitaPeli2);
        vaikeustasonValinta.AddItemHandler(2, AloitaPeli3);
        vaikeustasonValinta.AddItemHandler(3, ValitseMaila);
        Add(vaikeustasonValinta);
    }

    private void LuoEsteet1()
    {

    }

    private void LuoEsteet2()
    {

    }

    private void LuoMailatSuora()
    {
        maila1 = LuoMaila1(Level.Left + 90.0, 50.0);
        maila2 = LuoMaila1(Level.Right - 90.0, 50.0);
    }

    private void LuoMailatPyöreä()
    {
        maila1 = LuoMaila2(Level.Left + 90.0, 50.0);
        maila2 = LuoMaila2(Level.Right - 90.0, 50.0);
    }

    private void AloitaPeli1()
    {
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi);

    }

    private void AloitaPeli2()
    {
        Vector impulssi = new Vector(750.0, 0.0);
        pallo.Hit(impulssi);

    }

    private void AloitaPeli3()
    {
        Vector impulssi = new Vector(1000.0, 0.0);
        pallo.Hit(impulssi);

    }

    private void LuoKentta()
    {
        Level.Background.Color = Color.Black;
        
        Surface ylalaita = new Surface(930, 30);
        ylalaita.Y = Level.Top - 40;
        ylalaita.Restitution = 1;
        ylalaita.KineticFriction = 0;
        Add(ylalaita);

        Surface alalaita = new Surface(930, 30);
        alalaita.Y = Level.Bottom + 150;
        alalaita.Restitution = 1;
        alalaita.KineticFriction = 0;
        Add(alalaita);

        Surface oikealaita1 = new Surface(30, 200);
        oikealaita1.X = Level.Right - 50;
        oikealaita1.Y = ylalaita.Y - 100;
        oikealaita1.Restitution = 1;
        oikealaita1.KineticFriction = 0;
        Add(oikealaita1);

        Surface oikealaita2 = new Surface(30, 200);
        oikealaita2.X = Level.Right - 50;
        oikealaita2.Y = alalaita.Y + 100;
        oikealaita2.Restitution = 1;
        oikealaita2.KineticFriction = 0;
        Add(oikealaita2);

        Surface vasenlaita1 = new Surface(30, 200);
        vasenlaita1.X = Level.Left + 50;
        vasenlaita1.Y = ylalaita.Y - 100;
        vasenlaita1.Restitution = 1;
        vasenlaita1.KineticFriction = 0;
        Add(vasenlaita1);

        Surface vasenlaita2 = new Surface(30, 200);
        vasenlaita2.X = Level.Left + 50;
        vasenlaita2.Y = alalaita.Y + 100;
        vasenlaita2.Restitution = 1;
        vasenlaita2.KineticFriction = 0;
        Add(vasenlaita2);

        pallo = new PhysicsObject(30.0, 30.0);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Yellow;
        pallo.X = 0.0;
        pallo.Y = 50;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.MomentOfInertia = Double.PositiveInfinity;
        Add(pallo);
    }

    private PhysicsObject LuoMaila1(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
        maila.Shape = Shape.Rectangle;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        maila.KineticFriction = 0.0;
        Add(maila);
        return maila;
    }

    private PhysicsObject LuoMaila2(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(70, 100.0);
        maila.Shape = Shape.Ellipse;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        maila.KineticFriction = 0.0;
        Add(maila);
        return maila;
    }
}