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
        kentänValinta.AddItemHandler(0, LuoKentta1);
        kentänValinta.AddItemHandler(0, ValitseMaila);
        kentänValinta.AddItemHandler(1, LuoKentta2);
        kentänValinta.AddItemHandler(1, ValitseMaila);
        kentänValinta.AddItemHandler(2, LuoKentta3);
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

    private void LuoKentta1()
    {

    }

    private void LuoKentta2()
    {

    }

    private void LuoKentta3()
    {

    }


    private void LuoMailatSuora()
    {

    }
    private void LuoMailatPyöreä()
    {

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
        pallo = new PhysicsObject(30.0, 30.0);
        pallo.Shape = Shape.Circle;
        pallo.X = 0.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.MomentOfInertia = Double.PositiveInfinity;
        Add(pallo);
        Surface alaReuna = Surface.CreateBottom(Level);
        Add(alaReuna);
        maila1 = LuoMaila(-50, 0);
        maila2 = LuoMaila(50, 0);
    }

    private PhysicsObject LuoMaila(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
        maila.Shape = Shape.Rectangle;
        return maila;
    }
}

