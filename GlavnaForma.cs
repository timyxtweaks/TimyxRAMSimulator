using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimyxRAMSimulator
{
    public partial class GlavnaForma : Form
    {
        private Button dugmeOptimizacija;
        private ProgressBar progresTraka;
        private Label labelStatus;
        private Label labelMemorija;
        private Label labelPotpis;
        private Label labelNaslov;
        private Panel panelGlavni;
        private Random nasumicniBroj;
        private Timer tajmerAnimacija;
        private int trenutniMemorija;
        private int brojIteracija;
        private bool optimizacijaUToku;

        public GlavnaForma()
        {
            InitializeComponent();
            nasumicniBroj = new Random();
            trenutniMemorija = nasumicniBroj.Next(60, 95);
            optimizacijaUToku = false;
        }

        private void InitializeComponent()
        {
            this.dugmeOptimizacija = new Button();
            this.progresTraka = new ProgressBar();
            this.labelStatus = new Label();
            this.labelMemorija = new Label();
            this.labelPotpis = new Label();
            this.labelNaslov = new Label();
            this.panelGlavni = new Panel();
            this.tajmerAnimacija = new Timer();
            
            this.SuspendLayout();

            // Glavna forma
            this.Text = "TimyxRAMSimulator v1.0";
            this.Size = new Size(550, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Icon = KreirajIkonu();

            // Panel glavni
            this.panelGlavni.Size = new Size(500, 320);
            this.panelGlavni.Location = new Point(25, 25);
            this.panelGlavni.BackColor = Color.FromArgb(37, 37, 38);
            this.panelGlavni.BorderStyle = BorderStyle.FixedSingle;

            // Naslov
            this.labelNaslov.Text = "TIMYX RAM SIMULATOR";
            this.labelNaslov.Size = new Size(450, 40);
            this.labelNaslov.Location = new Point(25, 20);
            this.labelNaslov.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.labelNaslov.ForeColor = Color.FromArgb(0, 122, 204);
            this.labelNaslov.TextAlign = ContentAlignment.MiddleCenter;

            // Dugme za optimizaciju
            this.dugmeOptimizacija.Text = "POKRENI OPTIMIZACIJU";
            this.dugmeOptimizacija.Size = new Size(250, 60);
            this.dugmeOptimizacija.Location = new Point(125, 80);
            this.dugmeOptimizacija.BackColor = Color.FromArgb(0, 122, 204);
            this.dugmeOptimizacija.ForeColor = Color.White;
            this.dugmeOptimizacija.FlatStyle = FlatStyle.Flat;
            this.dugmeOptimizacija.FlatAppearance.BorderSize = 0;
            this.dugmeOptimizacija.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.dugmeOptimizacija.Cursor = Cursors.Hand;
            this.dugmeOptimizacija.Click += new EventHandler(this.DugmeOptimizacija_Click);

            // Progress traka
            this.progresTraka.Size = new Size(350, 25);
            this.progresTraka.Location = new Point(75, 160);
            this.progresTraka.Style = ProgressBarStyle.Continuous;
            this.progresTraka.Visible = false;
            this.progresTraka.ForeColor = Color.FromArgb(0, 122, 204);

            // Label za status
            this.labelStatus.Text = "Sistem spreman za optimizaciju memorije";
            this.labelStatus.Size = new Size(450, 30);
            this.labelStatus.Location = new Point(25, 200);
            this.labelStatus.Font = new Font("Segoe UI", 10F);
            this.labelStatus.ForeColor = Color.White;
            this.labelStatus.TextAlign = ContentAlignment.MiddleCenter;

            // Label za memoriju
            this.labelMemorija.Text = $"Trenutno zauzece RAM memorije: {trenutniMemorija}%";
            this.labelMemorija.Size = new Size(450, 30);
            this.labelMemorija.Location = new Point(25, 230);
            this.labelMemorija.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.labelMemorija.TextAlign = ContentAlignment.MiddleCenter;
            this.labelMemorija.ForeColor = Color.FromArgb(231, 76, 60);

            // Label za potpis
            this.labelPotpis.Text = "Napravio: timyx";
            this.labelPotpis.Size = new Size(150, 20);
            this.labelPotpis.Location = new Point(325, 280);
            this.labelPotpis.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.labelPotpis.ForeColor = Color.FromArgb(149, 165, 166);
            this.labelPotpis.TextAlign = ContentAlignment.MiddleRight;

            // Tajmer za animaciju
            this.tajmerAnimacija.Interval = 300;
            this.tajmerAnimacija.Tick += new EventHandler(this.TajmerAnimacija_Tick);

            // Dodavanje kontrola na panel
            this.panelGlavni.Controls.Add(this.labelNaslov);
            this.panelGlavni.Controls.Add(this.dugmeOptimizacija);
            this.panelGlavni.Controls.Add(this.progresTraka);
            this.panelGlavni.Controls.Add(this.labelStatus);
            this.panelGlavni.Controls.Add(this.labelMemorija);
            this.panelGlavni.Controls.Add(this.labelPotpis);

            // Dodavanje panela na formu
            this.Controls.Add(this.panelGlavni);

            this.ResumeLayout(false);
        }

        private Icon KreirajIkonu()
        {
            // Kreiranje jednostavne ikone
            Bitmap bitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.FromArgb(0, 122, 204));
                g.FillRectangle(Brushes.White, 8, 8, 16, 16);
                g.DrawString("R", new Font("Arial", 12, FontStyle.Bold), Brushes.Blue, 10, 8);
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private async void DugmeOptimizacija_Click(object sender, EventArgs e)
        {
            if (!optimizacijaUToku)
            {
                await PokreniLazenuOptimizaciju();
            }
        }

        private async Task PokreniLazenuOptimizaciju()
        {
            optimizacijaUToku = true;
            
            // Onemoguci dugme i promijeni tekst
            dugmeOptimizacija.Enabled = false;
            dugmeOptimizacija.Text = "OPTIMIZACIJA U TOKU...";
            dugmeOptimizacija.BackColor = Color.FromArgb(149, 165, 166);

            // Prikazi progress traku
            progresTraka.Visible = true;
            progresTraka.Value = 0;

            // Postavi pocetne vrijednosti
            brojIteracija = 0;
            labelStatus.Text = "Inicijalizacija optimizatora";

            // Pokreni animaciju
            tajmerAnimacija.Start();

            // Simulacija optimizacije sa realnim koracima
            var koraci = new[]
            {
                ("Skeniranje aktivnih procesa", 15),
                ("Analiza memorijskih blokova", 25),
                ("Identifikacija nepotrebnih podataka", 35),
                ("Oslobadanje cache memorije", 50),
                ("Defragmentacija RAM-a", 65),
                ("Optimizacija registra", 75),
                ("Kompresija podataka", 85),
                ("Finalizacija procesa", 95),
                ("Optimizacija zavrsena", 100)
            };

            foreach (var (opis, progres) in koraci)
            {
                labelStatus.Text = opis;
                
                // Postupno povecanje progress bara
                while (progresTraka.Value < progres)
                {
                    progresTraka.Value = Math.Min(progresTraka.Value + nasumicniBroj.Next(1, 3), progres);
                    await Task.Delay(nasumicniBroj.Next(50, 150));
                }
                
                await Task.Delay(nasumicniBroj.Next(200, 600));
            }

            // Zavrsi optimizaciju
            progresTraka.Value = 100;
            tajmerAnimacija.Stop();
            
            int novaMemorija = nasumicniBroj.Next(15, 35);
            int oslobodenoMB = nasumicniBroj.Next(800, 2500);
            int oslobodenoGB = oslobodenoMB / 1024;
            
            labelStatus.Text = $"USPJESNO! Oslobodeno {oslobodenoMB} MB ({oslobodenoGB}.{oslobodenoMB % 1024 / 100} GB) memorije";
            labelMemorija.Text = $"Trenutno zauzece RAM memorije: {novaMemorija}%";
            labelMemorija.ForeColor = Color.FromArgb(46, 204, 113);

            await Task.Delay(3000);

            // Resetuj kontrole
            progresTraka.Visible = false;
            dugmeOptimizacija.Enabled = true;
            dugmeOptimizacija.Text = "POKRENI OPTIMIZACIJU";
            dugmeOptimizacija.BackColor = Color.FromArgb(0, 122, 204);
            labelStatus.Text = "Sistem spreman za optimizaciju memorije";
            
            // Postavi novu vrijednost memorije za sljedecu optimizaciju
            trenutniMemorija = nasumicniBroj.Next(55, 90);
            labelMemorija.Text = $"Trenutno zauzece RAM memorije: {trenutniMemorija}%";
            labelMemorija.ForeColor = Color.FromArgb(231, 76, 60);
            
            optimizacijaUToku = false;
        }

        private void TajmerAnimacija_Tick(object sender, EventArgs e)
        {
            brojIteracija++;
            
            // Animacija sa tacicama
            string baznaPortuka = labelStatus.Text;
            if (baznaPortuka.Contains("..."))
            {
                baznaPortuka = baznaPortuka.Replace("...", "");
            }
            
            int brojTacaka = (brojIteracija % 4);
            string tacke = new string('.', brojTacaka);
            
            labelStatus.Text = baznaPortuka + tacke;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (optimizacijaUToku)
            {
                var rezultat = MessageBox.Show(
                    "Optimizacija je u toku. Da li ste sigurni da zelite zatvoriti aplikaciju?",
                    "TimyxRAMSimulator",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                
                if (rezultat == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            
            base.OnFormClosing(e);
        }
    }
}