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
    private const int nopeus = 300;
    private PhysicsObject pallo;
    private PhysicsObject maila1;
    private PhysicsObject maila2;
    
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
    /// Valikko vie myös vaikeus asteen valinnan valikkoon.
    ///Lisäksi vaihtoehtona palata aiempaan "käynnistysvalikkoon".
    /// </summary>
    private void ValitseKentta()
    {
        MultiSelectWindow kentanValinta = new MultiSelectWindow("Valitse Kenttä",
            "Tyhjä kenttä", "Palloja", "Piikkejä", "Takaisin");
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


    /// <summary>
    /// Valinnan 1 mukaiset esteet taulukossa. Kutsuu taulukkoa läpikäyvää aiohjelmaa TeeKentta.
    /// </summary>
    private void LuoEsteet1()
    {
        char[,] kentta = {
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'p', '.', '.', '.', '.', '.', 'p', '.'},
                {'.', '.', '.', '.', 'p', '.', '.', '.', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', '.', '.', '.', 'p', '.', '.', '.', '.'},
                {'.', 'p', '.', '.', '.', '.', '.', 'p', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                         };
        TeeKentta(kentta);
    }


    /// <summary>
    /// Valinnan 2 mukaiset esteet taulukossa. Kutsuu taulukkoa läpikäyvää aiohjelmaa TeeKentta.
    /// </summary>
    private void LuoEsteet2()
    {
        char[,] kentta = {
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'k', '.', '.', 'k', '.', '.', 'k', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', '.', 'k', '.', '.', '.', 'k', '.', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'k', '.', '.', 'k', '.', '.', 'k', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                         };
        TeeKentta(kentta);
    }


    /// <summary>
    /// käy läpi valitun kenttätaulukon alkioita  ja luo p:n paikalle pallon ja k:n paikalle kolmion
    /// </summary>
    /// <param name="kentta">valittu kenttä taulukkona</param>
    public void TeeKentta(char[,] kentta)
    {
        double kokox = 105 ;
        double kokoy = 91;
        double alkuX = vasenKeski.X +60;
        double alkuY = ylaLaita.Y -30;
        double y = alkuY;
        for (int iy = 0; iy < kentta.GetLength(0); iy++)
        {
        
            double x = alkuX;
            for (int ix = 0; ix < kentta.GetLength(1); ix++)
            {
                char t = kentta[iy, ix];

                switch (t)
                {
                    case 'p':
                        estePallo(x, y);
                        break;
                }

                switch (t)
                {
                    case 'k':
                        esteKolmio(x, y);
                        break;
                }
                x += kokox;
            }
            y -= kokoy;
        }
    }


    /// <summary>
    /// luo kentän esteenä olevan pallon
    /// </summary>
    /// <param name="x">pallon x kordinaatti</param>
    /// <param name="y">pallon y kordinaatti</param>
    private void estePallo(double x, double y)
    {
        int  r = 50;
        PhysicsObject pallo = PhysicsObject.CreateStaticObject(r, r);
        pallo.Shape = Shape.Circle;
        pallo.X = x;
        pallo.Y = y;
        pallo.Restitution = 1;
        pallo.KineticFriction = 0;
        pallo.Color = Color.Yellow;
        Add(pallo);
    }


    /// <summary>
    /// luo kentän esteenä olevan tasasivuisen kolmion
    /// </summary>
    /// <param name="x">pallon x kordinaatti</param>
    /// <param name="y">pallon y kordinaatti</param>
    private void esteKolmio(double x, double y)
    {
        int leveys = 70;
        int korkeus = 60;
        PhysicsObject esteKolmio = PhysicsObject.CreateStaticObject(leveys, korkeus);
        esteKolmio.Shape = Shape.Triangle;
        esteKolmio.X = x;
        esteKolmio.Y = y;
        esteKolmio.Restitution = 1;
        esteKolmio.KineticFriction = 0;
        esteKolmio.Color = Color.Yellow;
        esteKolmio.MomentOfInertia = 5000;
        Add(esteKolmio);
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


    /// <summary>
    /// Kutsuu aliohjelmaa ArvoSuunta joka palauttaa totuusarvon lyönnin x- ja  y- suunnan etumerkistä sekä kohdistaa palloon niiden mukaisen impulssin
    /// </summary>
    private void aloitusLyonti1()
    {
        int vauhtiY = 300;
        int vauhtiX = 300;
        if (ArvoSuunta() == false) vauhtiX = vauhtiX * -1;
        if (ArvoSuunta() == false) vauhtiY = vauhtiY * -1;
        Vector impulssi = new Vector(vauhtiX, vauhtiY);
        pallo.Hit(impulssi);
        Keyboard.Disable(Key.Space);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa ArvoSuunta joka palauttaa totuusarvon lyönnin x- ja  y- suunnasta sekä kohdistaa palloon niiden mukaisen impulssin
    /// </summary>
    private void aloitusLyonti2()
    {
        int vauhtiY = 400;
        int vauhtiX = 400;
        if (ArvoSuunta() == false) vauhtiX = vauhtiX * -1;
        if (ArvoSuunta() == false) vauhtiY = vauhtiY * -1;
        Vector impulssi = new Vector(vauhtiX, vauhtiY);
        pallo.Hit(impulssi);
        Keyboard.Disable(Key.Space);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa ArvoSuunta joka palauttaa totuusarvon lyönnin x- ja  y- suunnasta sekä kohdistaa palloon niiden mukaisen impulssin
    /// </summary>
    private void aloitusLyonti3()
    {
        int vauhtiY = 500;
        int vauhtiX = 500;
        if (ArvoSuunta() == false) vauhtiX = vauhtiX * -1;
        if (ArvoSuunta() == false) vauhtiY = vauhtiY * -1;
        Vector impulssi = new Vector(vauhtiX, vauhtiY);
        pallo.Hit(impulssi);
        Keyboard.Disable(Key.Space);
    }


    /// <summary>
    /// arpoo luvun 0 tai 1 joista toinen palauttaa true ja toinen false
    /// </summary>
    /// <returns>totuusarvo jota käytytetään positiivisena tai negatiivisena suuntana</returns>
    private static bool ArvoSuunta()
    {
        Random rand = new Random();
        int suunta = rand.Next(0, 2);
        if (suunta == 1) return false;
        return true;
    }


    /// <summary>
    /// Luodaan kentän rajat ja pallo
    /// </summary>
    private void LuoKentta()
    {
        const int ylaRaja = 360;
        const int alaRaja = -250;
        const int seinanPaksuus = 30;
        const int kentanLeveys = 930;
        const int kentankorkeus = 690;        
        const int kentanKeskiY = (ylaRaja + alaRaja) / 2;
        const int mailan1X = -410;
        const int mailan2X = 410;
        const int kulmaVino1 = 45;
        const int kulmaVino2 = -45;
        const int kulmaSuora = 0;
        Level.Background.Color = Color.Black;

        PeliPallo();

        ylaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, ylaRaja, kulmaSuora);
        alaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, alaRaja, kulmaSuora);
        PhysicsObject oikealaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, Level.Right - 50, ylaLaita.Y - 60, kulmaSuora);
        PhysicsObject oikealaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, Level.Right - 50, alaLaita.Y + 60, kulmaSuora);
        oikeaKeski = LuoSeina(seinanPaksuus, 3 * kentankorkeus / 5, Level.Right - 20, kentanKeskiY, kulmaSuora);
        PhysicsObject vasenlaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, Level.Left + 50, ylaLaita.Y - 60, kulmaSuora);
        PhysicsObject vasenlaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, Level.Left + 50, alaLaita.Y + 60, kulmaSuora);
        vasenKeski = LuoSeina(seinanPaksuus, 3*kentankorkeus / 5, Level.Left + 20, kentanKeskiY, kulmaSuora);
        PhysicsObject oikeaylä = LuoSeina(seinanPaksuus, 50, oikealaita1.X - 20, ylaLaita.Y - 20, kulmaVino1);
        PhysicsObject oikeaala = LuoSeina(seinanPaksuus, 50, oikealaita1.X - 20, alaLaita.Y + 20, kulmaVino2);
        PhysicsObject vasenaylä = LuoSeina(seinanPaksuus, 50, vasenlaita1.X + 20, ylaLaita.Y - 20, kulmaVino2);
        PhysicsObject vasenaala = LuoSeina(seinanPaksuus, 50, vasenlaita1.X + 20, alaLaita.Y + 20, kulmaVino1);

        maila1 = LuoMaila(mailan1X, kentanKeskiY);
        maila2 = LuoMaila(mailan2X, kentanKeskiY);
    }


    /// <summary>
    /// Luo pelipallon kentän keskelle
    /// </summary>
    private void PeliPallo()
    {
        const int ylaRaja = 360;
        const int alaRaja = -250;
        const int kentanKeskiY = (ylaRaja + alaRaja) / 2;
        const int kentanKeskiX = 0;
        const int pallonHalkaisija = 30;
        pallo = new PhysicsObject(pallonHalkaisija, pallonHalkaisija);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Red;
        pallo.X = kentanKeskiX;
        pallo.Y = kentanKeskiY;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.CanRotate = true;
        AddCollisionHandler(pallo, KasittelePallonTormays);
        Add(pallo);
        return;
    }


    /// <summary>
    /// Luo seinän
    /// </summary>
    /// <param name="pituus">seinän koko y-suunnassa</param>
    /// <param name="leveys">seinän koko x-suunnassa</param>
    /// <param name="sijaintiX">seinän keskipisteen x-kordinaatti</param>
    /// <param name="sijaintiY">seinän keskipisteen y-kordinaatti</param>
    /// <param name="kulma">seinän kulma asteina</param>
    /// <returns>parametrien mukainen seinä</returns>
    private PhysicsObject LuoSeina(double pituus, double leveys, double sijaintiX, double sijaintiY, int kulma)
    {
        PhysicsObject seina = new Surface(pituus, leveys);
        seina.X = sijaintiX;
        seina.Y = sijaintiY;
        seina.Restitution = 1;
        seina.KineticFriction = 0;
        seina.Color = Color.Red;
        seina.Angle = Angle.FromDegrees(kulma);
        Add(seina);
        return seina;
    }


/// <summary>
/// luo mailan
/// </summary>
/// <param name="x">mailan x-kordinaatti</param>
/// <param name="y">mailan y-kordinaatti</param>
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

    /// <summary>
    /// Kutsuu laskureita luovaa aliohjelmaa ja sijoittaa luodut laskurit muuttujiin. 
    /// </summary>
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
    private IntMeter LuoPisteLaskuri(double x, double y)
    {
        IntMeter laskuri = new IntMeter(0);
        laskuri.MaxValue = 5;
        laskuri.UpperLimit += Voitto;
        Label naytto = new Label();
        naytto.Font = new Font(70);
        naytto.BindTo(laskuri);
        naytto.X = x;
        naytto.Y = y;
        naytto.Height = 60;
        naytto.Width = 100;
        naytto.TextColor = Color.Yellow;
        naytto.BorderColor = Color.Yellow;
        naytto.Color = Level.BackgroundColor;
        Add(naytto);
        return laskuri;
    }


    /// <summary>
    /// Tapahtumat toisen pelaajan saavuttaessa 5p
    /// -onnitteluteksti
    /// -pallon poisto
    /// -pelin sulkeminen
    /// </summary>
    private void Voitto()
    {
        Label naytto = new Label();
        naytto.Font = new Font(70);
        naytto.X = 0;
        naytto.Y = 125;
        naytto.TextColor = Color.Yellow;
        naytto.BorderColor = Color.Yellow;
        naytto.Text = ("PELI PÄÄTTYI");
        Add(naytto);
        Remove(pallo);
        Keyboard.Disable(Key.Space);
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, LuoAloitusValikko, "Lopeta peli");
        Timer.SingleShot(5.0, Exit);
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
            Remove(pallo);
            PeliPallo();
            Keyboard.Enable(Key.Space);
        }
        else if (kohde == vasenKeski)
        {
            pelaajan2Pisteet.Value += 1;
            Remove(pallo);
            PeliPallo();
            Keyboard.Enable(Key.Space);
        }
    }


    /// <summary>
    /// Luodaan mailojen liikuttamiseen näppäimet sekä info ja pelin lopetus näppäin.
    /// </summary>
    private void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.A, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa ylös", maila1, new Vector(0, nopeus));
        Keyboard.Listen(Key.A, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.Z, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, new Vector(0, -nopeus));
        Keyboard.Listen(Key.Z, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa ylös", maila2, new Vector(0, nopeus));
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa alas", maila2, new Vector(0, -nopeus));
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

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