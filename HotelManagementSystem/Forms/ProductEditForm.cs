using System;
using System.IO;
using System.Windows.Forms;

private void btnImage_Click(object sender, EventArgs e)
{
    OpenFileDialog ofd = new OpenFileDialog();
    ofd.Filter = "Image|*.png;*.jpg";

    if (ofd.ShowDialog() == DialogResult.OK)
    {
        string dir = Application.StartupPath + "/ProductImages/";
        Directory.CreateDirectory(dir);

        string name = Path.GetFileName(ofd.FileName);
        string path = Path.Combine(dir, name);

        File.Copy(ofd.FileName, path, true);

        txtImage.Text = "ProductImages/" + name;
    }
}