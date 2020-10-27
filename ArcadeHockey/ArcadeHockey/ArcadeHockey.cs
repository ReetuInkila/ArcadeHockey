using System;
using System.Collections.Generic;
using System.Drawing.Text;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Microsoft.VisualBasic.FileIO;

public class ArcadeHockey : PhysicsGame
{
    private Vector nopeusYlos = new Vector(0, 300);
    private Vector nopeusAlas = new Vector(0, -300);
    private PhysicsObject pallo;
    private PhysicsObject maila1;
    private PhysicsObject maila2;
    private Random rand = new Random();
    private IntMeter pelaajan1Pisteet;
    private IntMeter pelaajan2Pisteet;
    private PhysicsObject oikeaKeski;
    private PhysicsObject vasenKeski;
    private PhysicsObject alaLaita;
    private PhysicsObject ylaLaita;


    /// <summary>
    /// Luodaan kentän samana pysyvät rakenteet sekä käynnistetään aliohjelmaketju jolla peliin valitaan 
    /// valikoilla muuttuvat rakenteet sekä käynnistetään itse peli.
    /// </summary>
    public override void Begin()
    {
        LuoKentta();
        LuoAloitusValikko();
        LisaaLaskurit();
        AsetaOhjaimet();
    }

    /// <summary>
    /// Valikko joka johtaa kentän esteiden valintaan tai sulkee pelin.
    /// </summary>
    private void LuoAloitusValikko()
    {
        MultiSelectWindow alkuValikko = new MultiSelectWindow("SpaceHockey", 
            "Aloita peli", "Lopeta");
        alkuValikko.AddItemHandler(0, ValitseKentta);
        alkuValikko.AddItemHandler(1, Exit);
        Add(alkuValikko);
    }
    
    /// <summary>
    /// Valikko joka johtaa kenttään ilman esteitä, kenttää jossa laidoissa on piikikkäitä esteitä tai kenttään jossa on palloja.
    /// Jokainen vaihtoehto lisäksi johtaa mailan tyypin valintaan. Lisäksi vaihtoehtona palata aiempaan "käynnistysvalikkoon".
    /// </summary>
    private void ValitseKentta()
    {
        MultiSelectWindow kentanValinta = new MultiSelectWindow("Valitse Kenttä",
            "Tyhjä kenttä", "Piikkejä", "Palloja", "Takaisin");
        kentanValinta.AddItemHandler(0, ValitseTaso);
        kentanValinta.AddItemHandler(1, LuoEsteet1);
        kentanValinta.AddItemHandler(1, ValitseTaso);
        kentanValinta.AddItemHandler(2, LuoEsteet2);
        kentanValinta.AddItemHandler(2, ValitseTaso);
        kentanValinta.AddItemHandler(3, LuoAloitusValikko);
        Add(kentanValinta);
    }
    

    /// <summary>
    /// Valikko joka käynnistää pelin eri nopeuksisilla palloilla. Lisäksi mahdollisuus palata aiempaan 
    /// mailojen valinta valikkoon. 
    /// </summary>
    private void ValitseTaso()
    {
        MultiSelectWindow vaikeustasonValinta = new MultiSelectWindow("Vaikeusaste",
            "Taso 1", "Taso 2", "Taso 3", "Palaa");
        vaikeustasonValinta.AddItemHandler(0, AloitaPeli1);
        vaikeustasonValinta.AddItemHandler(1, AloitaPeli2);
        vaikeustasonValinta.AddItemHandler(2, AloitaPeli3);
        vaikeustasonValinta.AddItemHandler(3, ValitseKentta);
        Add(vaikeustasonValinta);
    }
    ///TODO:Kentän esteet taulukoita käyttäen

    /// <summary>
    /// Luo kentälle valinnan 1 mukaiset esteet.
    /// </summary>
    private void LuoEsteet1()
    {

    }

    /// <summary>
    /// Luo kentälle valinnan 2 mukaiset esteet.
    /// </summary>
    private void LuoEsteet2()
    {

    }

    ///TODO: Space näppäimen poisto käytöstä aloituslyönnin jälkeen
    /// <summary>
    /// Käynnistää pelin välilyönnillä pallon nopeudella 1
    /// </summary>
    private void AloitaPeli1()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, aloitusLyonti1, "Aloita");
    }

    /// <summary>
    /// käynnistää pelin välilyönnillä pallon nopeudella 2
    /// </summary>
    private void AloitaPeli2()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, aloitusLyonti2, "Aloita");
    }

    /// <summary>
    /// Käynnistää pelin välilyönnillä nopeudella 3
    /// </summary>
    private void AloitaPeli3()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, aloitusLyonti3, "Aloita");
    }

    private void aloitusLyonti1()
    {
        int suuntaX = rand.Next(0, 1);
        int x = 250;
        if (suuntaX == 1) x = x * -1;
        int suuntaY = rand.Next(0, 1);
        int y = 250;
        if (suuntaY == 1) y = y * -1;
        Vector impulssi = new Vector(x, y);
        pallo.Hit(impulssi);
    }

    private void aloitusLyonti2()
    {
        int suuntaX = rand.Next(0, 1);
        int x = 350;
        if (suuntaX == 1) x = x * -1;
        int suuntaY = rand.Next(0, 1);
        int y = 350;
        if (suuntaY == 1) y = y * -1;
        Vector impulssi = new Vector(x, y);
        pallo.Hit(impulssi);
    }

    private void aloitusLyonti3()
    {
        int suuntaX = rand.Next(0, 1);
        int x = 450;
        if (suuntaX == 1) x = x * -1;
        int suuntaY = rand.Next(0, 1);
        int y = 450;
        if (suuntaY == 1) y = y * -1;
        Vector impulssi = new Vector(x, y);
        pallo.Hit(impulssi);
    }
    /// <summary>
    /// Luodaan kentän rajat ja pallo
    /// </summary>
    private void LuoKentta()
    {


        

        

        int kentanLeveys = 930;
        int kentankorkeus = 690;
        int seinanPaksuus = 30;
        int yla = 360;
        int ala = -250;
        int kentanKeskiY = (yla + ala) / 2;
        int kentanKeskiX = 0;
        int mailan1X = -410;
        int mailan2X = 410;
        int pallonHalkaisija = 30;
        Level.Background.Color = Color.Black;

        pallo = new PhysicsObject(pallonHalkaisija, pallonHalkaisija);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Yellow;
        pallo.X = kentanKeskiX;
        pallo.Y = kentanKeskiY;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.MomentOfInertia = Double.PositiveInfinity;
        AddCollisionHandler(pallo, KasittelePallonTormays);
        Add(pallo);

        ylaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, yla);
        alaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, ala);
        PhysicsObject oikealaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Right - 50, ylaLaita.Y - 100);
        PhysicsObject oikealaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Right - 50, alaLaita.Y + 100);
        oikeaKeski = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Right - 20, kentanKeskiY);
        PhysicsObject vasenlaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Left + 50, ylaLaita.Y - 100);
        PhysicsObject vasenlaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Left + 50, alaLaita.Y + 100);
        vasenKeski = LuoSeina(seinanPaksuus, kentankorkeus / 3, Level.Left + 20, kentanKeskiY);

        maila1 = LuoMaila(mailan1X, kentanKeskiY);
        maila2 = LuoMaila(mailan2X, kentanKeskiY);
    }

    private PhysicsObject LuoSeina(double pituus, double leveys, double sijaintiX, double sijaintiY)
    {
        PhysicsObject seina = new Surface(pituus, leveys);
        seina.X = sijaintiX;
        seina.Y = sijaintiY;
        seina.Restitution = 1;
        seina.KineticFriction = 0;
        seina.Color = Color.Red;
        Add(seina);
        return seina;
    }


/// <summary>
/// luo mailan
/// </summary>
/// <param name="x">mailan x-kordinaatti</param>
/// <param name="y">mailan y.kordinaatti</param>
/// <returns>ellipsin muotoinen maila</returns>
    private PhysicsObject LuoMaila(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(30, 100.0);
        maila.Shape = Shape.Ellipse;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        maila.KineticFriction = 0.0;
        Add(maila);
        return(maila);
    }

    private void LisaaLaskurit()
    {
        pelaajan1Pisteet = LuoPisteLaskuri(- 125.0, Screen.Bottom + 75.0);
        pelaajan2Pisteet = LuoPisteLaskuri(125.0, Screen.Bottom + 75.0);
    }

    /// <summary>
    /// luo annettuihin kordinaatteihin pistenäytöt
    /// </summary>
    /// <param name="x">pistenäytön x-kordinaatti</param>
    /// <param name="y">pistenäytön y-kordinaatti</param>
    /// <returns>pelaajan pisteemäärää kuvastava näyttö</returns>
    /// TODO: Pistelaskuri toimimaan!!! 
    /// TODO:Tapahtumat kun saadaan piste tai kun peli loppuu
    private IntMeter LuoPisteLaskuri(double x, double y)
    {
        Image[] kuvat = LoadImages("Numero0.gif", "Numero1.gif", "Numero2.gif", "Numero3.gif", "Numero4.gif", "Numero5.gif");
        IntMeter laskuri = new IntMeter(0);
        laskuri.MaxValue = 5;
        Label naytto = new Label();
        naytto.Image = kuvat[laskuri];
        naytto.BindTo(laskuri);
        naytto.TextColor = Color.Yellow;
        naytto.X = x;
        naytto.Y = y;
        naytto.Height = 60;
        naytto.Width = 40;
        naytto.BorderColor = Level.BackgroundColor;
        naytto.Color = Level.BackgroundColor;
        Add(naytto);
        return laskuri;
    }
/// <summary>
/// käy läpi pallon törmäyksiä ja lisää pistelaskurin arvoa maaliin osuessa
/// </summary>
/// <param name="pallo">liikkuva kohde</param>
/// <param name="kohde">kohde jojon törmätään</param>
    private void KasittelePallonTormays(PhysicsObject pallo, PhysicsObject kohde)
    {
        if (kohde == oikeaKeski)
        {
            pelaajan1Pisteet.Value += 1;
        }
        else if (kohde == vasenKeski)
        {
            pelaajan2Pisteet.Value += 1;
        }
    }

    /// <summary>
    /// Luodaan mailojen liikuttamiseen näppäimet sekä info ja pelin lopetus näppäin.
    /// </summary>
    /// TODO:Lopetusnäppäin johtamaan aloitusvalikkoon ja pelin pysäyttämiseen
    private void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.A, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa ylös", maila1, nopeusYlos);
        Keyboard.Listen(Key.A, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.Z, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, nopeusAlas);
        Keyboard.Listen(Key.Z, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa ylös", maila2, nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa alas", maila2, nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, LuoAloitusValikko, "Lopeta peli");

    }
/// <summary>
/// Pysäyttää mailan kun se on osumassa kentän rajoihin
/// </summary>
/// <param name="maila">maila jonka liikkumisnopeutta säädetään</param>
/// <param name="nopeus">vektori jonta käytetään nopeutena normaalitilanteissa</param>
    private void AsetaNopeus(PhysicsObject maila, Vector nopeus)
    {
        if ((nopeus.Y < 0) && (maila.Bottom < alaLaita.Top))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        if ((nopeus.Y > 0) && (maila.Top > ylaLaita.Bottom))
        {
            maila.Velocity = Vector.Zero;
            return;
        }

        maila.Velocity = nopeus;
    }
}