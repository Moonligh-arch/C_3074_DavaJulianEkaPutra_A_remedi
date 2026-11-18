using System;
using System.Collections.Generic;

// 1. class, object, attribute, dan method
public class Sekolah
{
    public string idSekolah { get; set; }
    public string namaSekolah { get; set; }
    public int jumlahPaketDiterima { get; private set; }

    public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima = 0)
    {
        this.idSekolah = idSekolah;
        this.namaSekolah = namaSekolah;
        this.jumlahPaketDiterima = jumlahPaketDiterima;
    }

    public void TambahPaketDiterima(int jumlah)
    {
        jumlahPaketDiterima += jumlah;
    }

    public void TampilInfo()
    {
        Console.WriteLine("id sekolah: " + idSekolah);
        Console.WriteLine("nama sekolah: " + namaSekolah);
        Console.WriteLine("jumlah paket diterima: " + jumlahPaketDiterima);
    }
}

// 2. encapsulation, 4. polymorphism, dan 5. abstract class
public abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    public int StokDapur
    {
        get { return stokDapur; }
        private set
        {
            if (value >= 0)
            {
                stokDapur = value;
            }
        }
    }

    protected PaketMBG(string idPaket, string namaPaket, int stokDapur)
    {
        IdPaket = idPaket;
        NamaPaket = namaPaket;
        StokDapur = stokDapur;
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > StokDapur)
        {
            return false;
        }

        StokDapur -= jumlah;
        return true;
    }

    public abstract int HitungBiaya(int jumlah);

    public virtual void TampilInfo()
    {
        Console.WriteLine("id paket: " + IdPaket);
        Console.WriteLine("nama paket: " + NamaPaket);
        Console.WriteLine("stok dapur: " + StokDapur);
    }
}

// 3. inheritance
public class PaketReguler : PaketMBG
{
    private const int HargaPerPaket = 15000;

    public PaketReguler(string idPaket, string namaPaket, int stokDapur)
        : base(idPaket, namaPaket, stokDapur)
    {
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * HargaPerPaket;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("harga per paket: Rp" + HargaPerPaket);
        Console.WriteLine("isi paket: nasi, ayam, sayur, dan buah");
    }
}

public class PaketKhusus : PaketMBG
{
    private const int HargaPerPaket = 20000;
    public string jenisDiet { get; set; }

    public PaketKhusus(string idPaket, string namaPaket, int stokDapur, string jenisDiet)
        : base(idPaket, namaPaket, stokDapur)
    {
        this.jenisDiet = jenisDiet;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * HargaPerPaket;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("harga per paket: Rp" + HargaPerPaket);
        Console.WriteLine("jenis diet: " + jenisDiet);
    }
}

// 6. interface
public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paketMakanan;
    private int jumlahPaket;

    public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paketMakanan, int jumlahPaket)
    {
        this.sekolahTujuan = sekolahTujuan;
        this.paketMakanan = paketMakanan;
        this.jumlahPaket = jumlahPaket;
    }

    public void KirimPaket()
    {
        Console.WriteLine("pengiriman paket sedang diproses...");

        if (jumlahPaket <= 0)
        {
            Console.WriteLine("distribusi gagal: jumlah paket harus lebih dari 0.");
            return;
        }

        if (jumlahPaket > paketMakanan.StokDapur)
        {
            Console.WriteLine("distribusi gagal: jumlah paket melebihi stok dapur yang tersedia.");
            return;
        }

        if (jumlahPaket > int.MaxValue - sekolahTujuan.jumlahPaketDiterima)
        {
            Console.WriteLine("distribusi gagal: jumlah paket diterima sekolah tidak dapat ditambah lagi.");
            return;
        }

        if (!paketMakanan.KurangiStok(jumlahPaket))
        {
            Console.WriteLine("distribusi gagal: stok paket tidak dapat dikurangi.");
            return;
        }

        sekolahTujuan.TambahPaketDiterima(jumlahPaket);
        Console.WriteLine("Distribusi MBG berhasil! Sebanyak " + jumlahPaket +
                          " paket MBG telah dikirim ke sekolah " + sekolahTujuan.namaSekolah + ".");
    }
}

// 7. opsional
public class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaket = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        daftarPaket.Add(paket);
    }

    public void TampilkanDataMBG()
    {
        Console.WriteLine("\ndata sekolah yang terdaftar:");
        foreach (Sekolah sekolah in daftarSekolah)
        {
            sekolah.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("data paket mbg yang tersedia:");
        foreach (PaketMBG paket in daftarPaket)
        {
            paket.TampilInfo();
            Console.WriteLine();
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("selamat datang di sistem distribusi mbg!\n");

        Sekolah sekolahA = new Sekolah("SKL-001", "SDN Harapan Bangsa");
        Sekolah sekolahB = new Sekolah("SKL-002", "SMP Cerdas Mandiri");

        PaketMBG paketReguler = new PaketReguler("PR-001", "paket reguler", 50);
        PaketMBG paketKhusus = new PaketKhusus("PK-001", "paket khusus", 30, "vegetarian");

        PengelolaMBG pengelola = new PengelolaMBG();
        pengelola.TambahSekolah(sekolahA);
        pengelola.TambahSekolah(sekolahB);
        pengelola.TambahPaket(paketReguler);
        pengelola.TambahPaket(paketKhusus);

        Console.WriteLine("informasi awal sekolah:");
        sekolahA.TampilInfo();
        Console.WriteLine();
        sekolahB.TampilInfo();

        Console.WriteLine("\ninformasi awal paket:");
        paketReguler.TampilInfo();
        Console.WriteLine();
        paketKhusus.TampilInfo();

        Console.WriteLine("biaya 10 paket reguler: Rp" + paketReguler.HitungBiaya(10));
        Console.WriteLine("biaya 10 paket khusus: Rp" + paketKhusus.HitungBiaya(10));

        DistribusiMBG distribusiBerhasil = new DistribusiMBG(sekolahA, paketReguler, 20);
        distribusiBerhasil.KirimPaket();
        Console.WriteLine("stok paket reguler sekarang: " + paketReguler.StokDapur);
        Console.WriteLine("paket yang diterima " + sekolahA.namaSekolah + ": " + sekolahA.jumlahPaketDiterima);

        int stokSebelumGagal = paketReguler.StokDapur;
        int diterimaSebelumGagal = sekolahA.jumlahPaketDiterima;
        DistribusiMBG distribusiGagal = new DistribusiMBG(sekolahA, paketReguler, 40);
        distribusiGagal.KirimPaket();
        Console.WriteLine("stok tetap: " + paketReguler.StokDapur);
        Console.WriteLine("paket diterima tetap: " + sekolahA.jumlahPaketDiterima);
        Console.WriteLine("data tidak berubah setelah gagal: " +
                          (stokSebelumGagal == paketReguler.StokDapur &&
                           diterimaSebelumGagal == sekolahA.jumlahPaketDiterima));

        Console.WriteLine("\nringkasan data akhir dari pengelola:");
        pengelola.TampilkanDataMBG();
    }
}
