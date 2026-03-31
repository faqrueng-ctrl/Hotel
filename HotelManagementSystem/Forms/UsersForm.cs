using HotelManagementSystem.Services;
using System.Windows.Forms;

private void LoadUsers()
{
    var service = new UserService();
    dataGridView1.DataSource = service.GetAll().ToList();
}