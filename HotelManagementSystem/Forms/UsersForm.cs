using System;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class UsersForm : Form
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly UserService service = new UserService();

        public UsersForm()
        {
            Text = "Пользователи";
            Width = 820;
            Height = 480;
            StartPosition = FormStartPosition.CenterParent;

            grid.SetBounds(10, 10, 780, 380);
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            var btnDelete = new Button { Left = 10, Top = 400, Width = 160, Text = "Удалить пользователя" };
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Visible = Session.IsAdmin;

            Controls.AddRange(new Control[] { grid, btnDelete });
            LoadUsers();
        }

        private void LoadUsers()
        {
            grid.DataSource = service.GetAll().Select(u => new
            {
                u.UserId,
                u.Name,
                u.Email,
                u.Phone,
                Role = u.Role.Name
            }).ToList();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.CurrentRow == null)
                {
                    return;
                }

                var id = Convert.ToInt32(grid.CurrentRow.Cells["UserId"].Value);
                service.Delete(id);
                LoadUsers();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
