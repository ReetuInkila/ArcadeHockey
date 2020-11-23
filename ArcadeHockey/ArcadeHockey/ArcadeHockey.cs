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
    private const int mailanNopeus = 300;
    private Label naytto;
    private PhysicsObject pallo;
    private PhysicsObject maila1;
    private PhysicsObject maila2;
    private PhysicsObject oikeaKeski;
    private PhysicsObject vasenKeski;
    private PhysicsObject alaLaita;
    private PhysicsObject ylaLaita;
    private IntMeter pelaajan1Pisteet;
    private IntMeter pelaajan2Pisteet;


    /// <summary>
    /// Luodaan pelin samana pysyvät rakenteet kutsumalla aliohjelmia LuoKentta, LisaaLaskurit ja AsetaOhjaimet 
    /// sekä käynnistetään "aliohjelmaketju" jolla peliin valitaan valikoilla muuttuvat rakenteet sekä käynnistetään itse peli.
    /// </summary>
    public override void Begin()
    {
        LuoKentta();
        LisaaLaskurit();
        AsetaOhjaimet();
        LuoAloitusValikko();
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
    /// Valikko joka johtaa kenttään ilman esteitä, kenttää jossa esteenä pyöriviä piikkejä tai kenttään jossa on palloja.
    /// Valikko vie myös vaikeus asteen valinnan valikkoon.
    /// Lisäksi vaihtoehtona palata aiempaan "käynnistysvalikkoon".
    /// </summary>
    private void ValitseKentta()
    {
        MultiSelectWindow kentanValinta = new MultiSelectWindow("Valitse Kenttä",
            "Tyhjä kenttä", "Palloja", "Piikikäs", "Takaisin");
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
    /// Valinnan palloja mukaiset esteet taulukossa. Kutsuu taulukkoa läpikäyvää aiohjelmaa TeeKentta.
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
    /// Valinnan Piikikäs mukaiset esteet taulukossa. Kutsuu taulukkoa läpikäyvää aiohjelmaa TeeKentta.
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
        int kokox = 105 ;
        int kokoy = 91;
        int alkuX = -420;
        int alkuY = 330;
        int y = alkuY;
        for (int iy = 0; iy < kentta.GetLength(0); iy++)
        {
        
            double x = alkuX;
            for (int ix = 0; ix < kentta.GetLength(1); ix++)
            {
                char t = kentta[iy, ix];

                switch (t)
                {
                    case 'p':
                        EstePallo(x, y);
                        break;
                }

                switch (t)
                {
                    case 'k':
                        EsteKolmio(x, y);
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
    private void EstePallo(double x, double y)
    {
        int  r = 50;
        PhysicsObject pallo = PhysicsObject.CreateStaticObject(r, r);
        pallo.Shape = Shape.Circle;
        pallo.X = x;
        pallo.Y = y;
        pallo.Restitution = 1;
        pallo.Color = Color.Yellow;
        Add(pallo);
    }


    /// <summary>
    /// luo kentän esteenä olevan tasasivuisen kolmion
    /// </summary>
    /// <param name="x">pallon x kordinaatti</param>
    /// <param name="y">pallon y kordinaatti</param>
    private void EsteKolmio(double x, double y)
    {
        int leveys = 70;
        int korkeus = 60;
        PhysicsObject esteKolmio = PhysicsObject.CreateStaticObject(leveys, korkeus);
        esteKolmio.Shape = Shape.Triangle;
        esteKolmio.X = x;
        esteKolmio.Y = y;
        esteKolmio.Restitution = 1;
        esteKolmio.Color = Color.Yellow;
        esteKolmio.MomentOfInertia = 5000;
        Add(esteKolmio);
    }


    /// <summary>
    /// Käynnistää pelin välilyönnillä pallon nopeudella 1 kutsumalla aliohjelmaa AloitusLyonti1
    /// </summary>
    private void AloitaPeli1()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, AloitusLyonti1, "Aloita");
    }


    /// <summary>
    /// käynnistää pelin välilyönnillä pallon nopeudella 2 kutsumalla aliohjelmaa AloitusLyonti2
    /// </summary>
    private void AloitaPeli2()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, AloitusLyonti2, "Aloita");
    }


    /// <summary>
    /// Käynnistää pelin välilyönnillä nopeudella 3 kutsumalla aliohjelmaa AloitusLyonti3
    /// </summary>
    private void AloitaPeli3()
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, AloitusLyonti3, "Aloita");
    }


    /// <summary>
    /// Kutsuu aliohjelmaa AloitusLyonti ja vie sille parametrina pallon nopeuden
    /// </summary>
    private void AloitusLyonti1()
    {
        AloitusLyonti(3);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa AloitusLyonti ja vie sille parametrina pallon nopeuden
    /// </summary>
    private void AloitusLyonti2()
    {
        AloitusLyonti(4);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa AloitusLyonti ja vie sille parametrina pallon nopeuden
    /// </summary>
    private void AloitusLyonti3()
    {
        AloitusLyonti(5);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa ArvoSuunta joka palauttaa totuusarvon lyönnin x- ja  y- suunnan etumerkistä sekä kohdistaa palloon niiden mukaisen impulssin
    /// </summary>
    /// <param name="vauhti"></param>
    private void AloitusLyonti(int vauhti)
    {
        int vauhtiY = vauhti * 100;
        int vauhtiX = vauhti * 100;
        if (ArvoSuunta() == false) vauhtiX *= -1;
        if (ArvoSuunta() == false) vauhtiY *= -1;
        Vector impulssi = new Vector(vauhtiX, vauhtiY);
        pallo.Hit(impulssi);
        Keyboard.Disable(Key.Space);
    }


    /// <summary>
    /// arpoo totuusarvon true tai flase 50/50 todennäköisyydellä
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
    /// Luodaan kentän rajat muodostavat kappaleet kutsumalla aliohjelmaa LuoSeina, pallo kutsumalla aliohjelmaa PeliPallo sekä 
    /// mailat kutsumalla aliohjelmaa LuoMaila
    /// </summary>
    private void LuoKentta()
    {
        int ylaRaja = 360;
        int alaRaja = -250;
        int seinanPaksuus = 30;
        int oikeaLaitaX = 450;
        int vasenLaitaX = -450;
        int oikeaMaaliX = oikeaLaitaX + seinanPaksuus;
        int vasenMaaliX = vasenLaitaX - seinanPaksuus;
        int kentanLeveys = 930;
        int kentankorkeus = 690; 
        int kentanKeskiY = (ylaRaja + alaRaja) / 2;
        int mailan1X = -410;
        int mailan2X = 410;
        Level.Background.Color = Color.Black;

        ylaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, ylaRaja);
        alaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, alaRaja);
        PhysicsObject oikealaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, oikeaLaitaX, ylaLaita.Y - 60);
        PhysicsObject oikealaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, oikeaLaitaX, alaLaita.Y + 60);
        oikeaKeski = LuoSeina(seinanPaksuus, 3 * kentankorkeus / 5, oikeaMaaliX, kentanKeskiY);
        PhysicsObject vasenlaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, vasenLaitaX, ylaLaita.Y - 60);
        PhysicsObject vasenlaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, vasenLaitaX, alaLaita.Y + 60);
        vasenKeski = LuoSeina(seinanPaksuus, 3*kentankorkeus / 5, vasenMaaliX, kentanKeskiY);

        maila1 = LuoMaila(mailan1X, kentanKeskiY);
        maila2 = LuoMaila(mailan2X, kentanKeskiY);

        pallo = PeliPallo();
    }


    /// <summary>
    /// Luo pelipallon kentän keskelle
    /// </summary>
    private PhysicsObject PeliPallo()
    {
        int kentanKeskiY = 55;
        int kentanKeskiX = 0;
        int pallonHalkaisija = 30;
        pallo = new PhysicsObject(pallonHalkaisija, pallonHalkaisija);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Red;
        pallo.X = kentanKeskiX;
        pallo.Y = kentanKeskiY;
        pallo.Restitution = 1.0;
        pallo.KineticFriction = 0.0;
        pallo.CanRotate = false;
        AddCollisionHandler(pallo, KasittelePallonTormays);
        Add(pallo);
        return pallo;
    }


    /// <summary>
    /// Luo seinän
    /// </summary>
    /// <param name="pituus">seinän koko y-suunnassa</param>
    /// <param name="leveys">seinän koko x-suunnassa</param>
    /// <param name="sijaintiX">seinän keskipisteen x-kordinaatti</param>
    /// <param name="sijaintiY">seinän keskipisteen y-kordinaatti</param>
    /// <returns>parametrien mukainen seinä</returns>
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
    /// Kutsuu laskureita luovaa aliohjelmaa ja sijoittaa niillä luodut laskurit muuttujiin. 
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
    /// -pelin sulkeminen tai uusi peli
    /// </summary>
    private void Voitto()
    {
        naytto = new Label
        {
            Font = new Font(70),
            X = 0,
            Y = 130,
            TextColor = Color.Yellow,
            BorderColor = Color.Yellow,
            Text = ("PELI PÄÄTTYI")
        };
        Add(naytto);        
        MultiSelectWindow uusiPeli = new MultiSelectWindow("SpaceHockey",
            "Uusi peli", "Lopeta");
        uusiPeli.AddItemHandler(0, UusiPeli);
        uusiPeli.AddItemHandler(1, Exit);
        Add(uusiPeli);
        return;
    }


    /// <summary>
    /// Nollaa oiste laskurit ja peli alkaa uudestaan
    /// </summary>
    private void UusiPeli()
    {
        pelaajan1Pisteet.SetValue(0);
        pelaajan2Pisteet.SetValue(0);
        Remove(naytto);
        Remove(pallo);
        Add(pallo);
        Keyboard.Enable(Key.Space);
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
    /// Luodaan mailojen liikuttamiseen näppäimet sekä info, pallonjumittumisen poisto ja pelin lopetus näppämen.
    /// </summary>
    private void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.A, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa ylös", maila1, new Vector(0, mailanNopeus));
        Keyboard.Listen(Key.A, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.Z, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, new Vector(0, -mailanNopeus));
        Keyboard.Listen(Key.Z, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa ylös", maila2, new Vector(0, mailanNopeus));
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa alas", maila2, new Vector(0, -mailanNopeus));
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.F2, ButtonState.Pressed, PalloJumittui, "Näytä ohjeet");

    }


    /// <summary>
    /// Kohdistaa palloon pienen voiman jolla ei vaikuteta peliin mutta saadaan pallo liikkeelle
    /// </summary>
    private void PalloJumittui()
    {
        Vector impulssi = new Vector(5, 0);
        pallo.Hit(impulssi);
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