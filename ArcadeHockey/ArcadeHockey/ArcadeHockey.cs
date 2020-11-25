using System;
using Jypeli;

/// @author  Lauri Reetu Taavetti Inkilä
/// @version 23.11.2020
/// <summary>
/// Ilmakiekko peli 
/// </summary>
public class ArcadeHockey : PhysicsGame
{
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
            "Tyhjä kenttä", "Palloja", "Piikikäs", "Lopeta");
        kentanValinta.AddItemHandler(0, ValitseTaso);
        kentanValinta.AddItemHandler(1, LuoEsteet, 1);
        kentanValinta.AddItemHandler(1, ValitseTaso);
        kentanValinta.AddItemHandler(2, LuoEsteet, 2);
        kentanValinta.AddItemHandler(2, ValitseTaso);
        kentanValinta.AddItemHandler(3, Exit);
        Add(kentanValinta);
    }
    

    /// <summary>
    /// Valikko joka käynnistää pelin eri nopeuksisilla palloilla. Lisäksi mahdollisuus palata aiempaan 
    /// mailojen valinta valikkoon. 
    /// </summary>
    private void ValitseTaso()
    {
        MultiSelectWindow vaikeustasonValinta = new MultiSelectWindow("Vaikeusaste",
            "Taso 1", "Taso 2", "Taso 3", "Lopeta");
        vaikeustasonValinta.AddItemHandler(0, AloitaPeli, 300);
        vaikeustasonValinta.AddItemHandler(1, AloitaPeli, 400);
        vaikeustasonValinta.AddItemHandler(2, AloitaPeli, 500);
        vaikeustasonValinta.AddItemHandler(3, Exit);
        Add(vaikeustasonValinta);
    }


    /// <summary>
    /// kentät taulukoissa joista parametrin mukainen valitaan aliohjelmakutsuun tee kenttä
    /// </summary>
    /// <param name="kentta">valitun kentän numero</param>
    private void LuoEsteet(int kentta)
    {
        char[,] kentta1 = {
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'p', '.', '.', '.', '.', '.', 'p', '.'},
                {'.', '.', '.', '.', 'p', '.', '.', '.', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', '.', '.', '.', 'p', '.', '.', '.', '.'},
                {'.', 'p', '.', '.', '.', '.', '.', 'p', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                            };
        
        char[,] kentta2 = {
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'k', '.', '.', 'k', '.', '.', 'k', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', '.', 'k', '.', '.', '.', 'k', '.', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                {'.', 'k', '.', '.', 'k', '.', '.', 'k', '.'},
                {'.', '.', '.', '.', '.', '.', '.', '.', '.'},
                            };
        if (kentta == 1) TeeKentta(kentta1);
        if (kentta == 2) TeeKentta(kentta2);
    }


    /// <summary>
    /// käy läpi valitun kenttätaulukon alkioita  ja luo p:n paikalle pallon ja k:n paikalle kolmion
    /// </summary>
    /// <param name="kentta">valittu kenttä taulukkona</param>
    public void TeeKentta(char[,] kentta)
    {
        const int kokox = 105 ;
        const int kokoy = 91;
        const int alkuX = -420;
        const int alkuY = 330;
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
        const int r = 50;
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
        const int leveys = 70;
        const int korkeus = 60;
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
    /// Aloituslyönti välilyönnillä käyttäen aliohjelmaa AloitusLyonti
    /// </summary>
    /// <param name="nopeus">pallon nopeutena käytettävä nopeus</param>
    private void AloitaPeli(int nopeus)
    {
        Keyboard.Listen(Key.Space, ButtonState.Pressed, AloitusLyonti, "Aloita", nopeus);
    }


    /// <summary>
    /// Kutsuu aliohjelmaa ArvoSuunta joka palauttaa totuusarvon lyönnin x- ja  y- suunnan etumerkistä sekä kohdistaa palloon 
    /// niiden mukaisen impulssin
    /// </summary>
    /// <param name="vauhti">pallon nopeus</param>
    private void AloitusLyonti(int vauhti)
    {
        int vauhtiY = vauhti;
        int vauhtiX = vauhti;
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
        const int ylaRaja = 360;
        const int alaRaja = -250;
        const int seinanPaksuus = 30;
        const int oikeaLaitaX = 450;
        const int vasenLaitaX = -450;
        const int oikeaMaaliX = oikeaLaitaX + seinanPaksuus;
        const int vasenMaaliX = vasenLaitaX - seinanPaksuus;
        const int kentanLeveys = 930;
        const int kentankorkeus = 690; 
        const int kentanKeskiY = (ylaRaja + alaRaja) / 2;
        const int mailan1X = -410;
        const int mailan2X = 410;
        const int paatyYlaY = 300;
        const int paatyAlaY = -190;
        Level.Background.Color = Color.Black;

        ylaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, ylaRaja);
        alaLaita = LuoSeina(kentanLeveys, seinanPaksuus, 0, alaRaja);
        PhysicsObject oikealaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, oikeaLaitaX, paatyYlaY);
        PhysicsObject oikealaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, oikeaLaitaX, paatyAlaY);
        oikeaKeski = LuoSeina(seinanPaksuus, 3 * kentankorkeus / 5, oikeaMaaliX, kentanKeskiY);
        PhysicsObject vasenlaita1 = LuoSeina(seinanPaksuus, kentankorkeus / 5, vasenLaitaX, paatyYlaY);
        PhysicsObject vasenlaita2 = LuoSeina(seinanPaksuus, kentankorkeus / 5, vasenLaitaX, paatyAlaY);
        vasenKeski = LuoSeina(seinanPaksuus, 3*kentankorkeus / 5, vasenMaaliX, kentanKeskiY);

        maila1 = LuoMaila(mailan1X, kentanKeskiY);
        maila2 = LuoMaila(mailan2X, kentanKeskiY);

        pallo = PeliPallo();

        Label ohjeet = new Label();
        ohjeet.Font = new Font(20);
        ohjeet.X = 0;
        ohjeet.Y = -300;
        ohjeet.TextColor = Color.Yellow;
        ohjeet.BorderColor = Color.Yellow;
        ohjeet.Text = ("Ohjeet: Paina F1");
        Add(ohjeet);
    }


    /// <summary>
    /// Luo pelipallon kentän keskelle
    /// </summary>
    private PhysicsObject PeliPallo()
    {
        const int kentanKeskiY = 55;
        const int kentanKeskiX = 0;
        const int pallonHalkaisija = 30;
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
        const int laskuritY = -300;
        const int laskurit1X = -125;
        const int laskurit2X = 125;
        pelaajan1Pisteet = LuoPisteLaskuri(laskurit1X, laskuritY );
        pelaajan2Pisteet = LuoPisteLaskuri(laskurit2X, laskuritY);
    }


    /// <summary>
    /// Luo annettuihin kordinaatteihin pistenäytöt
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
        naytto.Font = new Font(60);
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
        Label naytto = new Label();
        naytto.Font = new Font(70);
        naytto.X = 0;
        naytto.Y = 310;
        naytto.TextColor = Color.Yellow;
        naytto.BorderColor = Color.Yellow;
        naytto.Text = ("PELI PÄÄTTYI");
        Add(naytto);        
        MultiSelectWindow uusiPeli = new MultiSelectWindow("SpaceHockey",
            "Uusi peli", "Lopeta");
        uusiPeli.AddItemHandler(0, UusiPeli, naytto);
        uusiPeli.AddItemHandler(1, Exit);
        Add(uusiPeli);
    }


    /// <summary>
    /// Nollaa piste laskurit ja peli alkaa uudestaan
    /// </summary>
    private void UusiPeli(Label naytto)
    {
        pelaajan1Pisteet.SetValue(0);
        pelaajan2Pisteet.SetValue(0);
        Remove(naytto);
        Remove(pallo);
        PeliPallo();
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
    /// Luodaan mailojen liikuttamiseen näppäimet sekä info, pallonjumittumisen poisto ja pelin lopetus näppämet.
    /// </summary>
    private void AsetaOhjaimet()
    {
        const int mailanNopeus = 300;
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
        Keyboard.Listen(Key.F2, ButtonState.Pressed, PalloJumittui, "Jumittuiko pallo");
        Keyboard.Listen(Key.F3, ButtonState.Pressed, PalloKatosi, "Katosiko pallo");

    }


    /// <summary>
    /// Kohdistaa palloon pienen voiman jolla ei vaikuteta peliin mutta saadaan pallo liikkeelle
    /// </summary>
    private void PalloJumittui()
    {
        Vector impulssi = new Vector(50, 10);
        pallo.Hit(impulssi);
    }


    /// <summary>
    /// Poistaa vanhan "karanneen" pallon ja luo uuden
    /// </summary>
    private void PalloKatosi()
    {
        Remove(pallo);
        PeliPallo();
        Keyboard.Enable(Key.Space);
    }


    /// <summary>
    /// Liikuttaa mailaa ja mailan kun se on osumassa kentän rajoihin
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