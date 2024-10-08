using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inventory
{
    public partial class frmAddProduct : Form
    {
        private string _ProductName, _Category, _MfgDate, _ExpDate, _Description;
        private int _Quantity;
        private double _SellPrice;
        private byte[] _ProductImage;

        BindingSource showProductList = new BindingSource();

        public frmAddProduct()
        {
            InitializeComponent();
        }

        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            string[] ListOfProductCategory = new string[]
            {
                "Beverage",
                "Bread/Bakery",
                "Canned/Jarred Goods",
                "Dairy",
                "Frozen Goods",
                "Meat",
                "Personal care",
                "Other",
            };

            foreach (string categories in ListOfProductCategory)
            {
                cbCategory.Items.Add(categories);
            }
            bool imageColumnExists = false;
            foreach (DataGridViewColumn column in gridViewProductList.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    imageColumnExists = true;
                    break;
                }
            }

            
            if (!imageColumnExists)
            {
                
            }
        }

        public string Product_Name(string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                //Exception here
                throw new StringFormatException("Invalid Input, Letters Only!");
                return name;
        }
        public int Quantity(string qty)
        {
            if (!Regex.IsMatch(qty, @"^[0-9]"))
                //Exception here
                throw new NumberFormatException("Invalid Input, Number Only!");
            return Convert.ToInt32(qty);
        }
        public double SellingPrice(string price)
        {
            if (!Regex.IsMatch(price.ToString(), @"^(\d*\.)?\d+$"))
                //Exception here
                throw new CurrencyFormatException("Invalid Input, Digit Only!");
            return Convert.ToDouble(price);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);

                    // Convert image to byte array
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                        _ProductImage = ms.ToArray();  // Store byte array for the image
                    }
                }
            }
        }

        public class NumberFormatException : Exception
        {
            public NumberFormatException(string str) : base(str) { }
        }
        public class StringFormatException : Exception
        {
            public StringFormatException(string str) : base(str) { }
        }
       
        public class CurrencyFormatException : Exception
        {
            public CurrencyFormatException(string str) : base(str) { }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                _ProductName = Product_Name(txtProductName.Text);
                _Category = cbCategory.Text;
                _MfgDate = dtPickerMfgDate.Value.ToString("yyyy-MM-dd");
                _ExpDate = dtPickerExpDate.Value.ToString("yyyy-MM-dd");
                _Description = richTxtDescription.Text;
                _Quantity = Quantity(txtQuantity.Text);
                _SellPrice = SellingPrice(txtSellPrice.Text);
                showProductList.Add(new ProductClass(_ProductName, _Category, _MfgDate,
                                             _ExpDate, _SellPrice, _Quantity, _Description, _ProductImage));

                gridViewProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridViewProductList.DataSource = showProductList;
            }
            catch (StringFormatException ex)
            {
                MessageBox.Show(ex.Message, "<<There's Something Wrong>>");
            }
            catch (NumberFormatException ex)
            {
                MessageBox.Show(ex.Message, "<<There's Something Wrong>>");
            }
            catch (CurrencyFormatException ex)
            {
                MessageBox.Show(ex.Message, "<<There's Something Wrong>>");
            }
        }
    }
}
