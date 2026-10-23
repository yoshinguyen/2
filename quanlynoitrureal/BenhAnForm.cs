using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form quản lý Hồ sơ Bệnh án nội trú:
    /// - Cho phép ghi nhận các lần thăm khám của bệnh nhân trong đợt nằm viện.
    /// - Mỗi lần khám tạo 1 bản ghi BENHAN mới (ngày lập, triệu chứng, chẩn đoán, kết quả khám, hướng điều trị).
    /// - Đồng thời tự động cập nhật tình trạng bệnh mới nhất vào bảng BENHNHAN.
    /// </summary>
    public partial class BenhAnForm : BaseForm
    {
        /// <summary>
        /// initiallize
        /// </summary>
        public BenhAnForm()
        {
            InitializeComponent(); 

            // fill
            FillCombo(cmbNV,
                DatabaseHelper.GetData(@"SELECT nv.MaNhapVien,
                                  nv.MaNhapVien + N' - ' + bn.HoTen + N' (' + nv.TrangThai + N')' AS Hien
                           FROM NHAPVIEN nv JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                           WHERE nv.TrangThai <> N'Hủy'
                           ORDER BY nv.NgayNhapVien DESC"),
                "Hien", "MaNhapVien");

            LoadData();

            // Khi đổi ca nhập viện, tải lại lịch sử bệnh án và xóa các ô nhập liệu
            cmbNV.SelectedIndexChanged += (s, e) => { LoadData(); ClearInputs(); };

            //dgv fill
            dgv.SelectionChanged += (s, e) => FillInputs();
        }

        /// <summary>
        /// refresh btn
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e)
        {
            ClearInputs();
        }

        /// <summary>
        /// load data
        /// </summary>
        void LoadData()
        {
            dgv.DataSource = DatabaseHelper.GetData(
                @"SELECT MaBA AS [Mã BA], NgayLap AS [Ngày lập], TrieuChung AS [Triệu chứng], ChanDoan AS [Chẩn đoán],
                         KetQuaKham AS [Kết quả khám], HuongDieuTri AS [Hướng điều trị]
                  FROM BENHAN WHERE MaNhapVien = @nv
                  ORDER BY NgayLap DESC, MaBA DESC", DatabaseHelper.Param("@nv", GetValue(cmbNV)));
            FormatColumn(dgv, "Ngày lập", "dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// dgv click fill
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;
            txtMa.Text = GetText(r, "Mã BA");
            txtTrieuChung.Text = GetText(r, "Triệu chứng");
            txtChanDoan.Text = GetText(r, "Chẩn đoán");
            txtKetQua.Text = GetText(r, "Kết quả khám");
            txtHuong.Text = GetText(r, "Hướng điều trị");
        }

        /// <summary>
        /// clear
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTrieuChung.Text = txtChanDoan.Text = txtKetQua.Text = txtHuong.Text = "";
            dgv.CurrentCell = null;
        }

        /// <summary>
        ///add
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            string nv = GetValue(cmbNV);
            if (nv == null) { ShowWarning("Hãy chọn hồ sơ nhập viện."); return; }
            if (txtChanDoan.Text.Trim() == "") { ShowWarning("Vui lòng nhập chẩn đoán."); txtChanDoan.Focus(); return; }

            try
            {
                string ma = DatabaseHelper.NextId("BENHAN", "MaBA", "BA");
                DatabaseHelper.ExecuteTransaction(
                    new SqlStatement(@"INSERT INTO BENHAN(MaBA, MaNhapVien, TrieuChung, ChanDoan, KetQuaKham, HuongDieuTri)
                               VALUES(@ma, @nv, @tc, @cd, @kq, @hdt)",
                        DatabaseHelper.Param("@ma", ma), DatabaseHelper.Param("@nv", nv), DatabaseHelper.Param("@tc", txtTrieuChung.Text.Trim()),
                        DatabaseHelper.Param("@cd", txtChanDoan.Text.Trim()), DatabaseHelper.Param("@kq", txtKetQua.Text.Trim()), DatabaseHelper.Param("@hdt", txtHuong.Text.Trim())),
                    new SqlStatement("UPDATE BENHNHAN SET TinhTrang = @cd2 WHERE MaBN = (SELECT MaBN FROM NHAPVIEN WHERE MaNhapVien = @nv2)",
                        DatabaseHelper.Param("@cd2", txtChanDoan.Text.Trim()), DatabaseHelper.Param("@nv2", nv)));
                LoadData();
                ClearInputs();
                ShowInfo("Đã lưu lần khám " + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        ///mod
        /// </summary>
        void btnCapNhat_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn một dòng bệnh án để cập nhật."); return; }
            if (txtChanDoan.Text.Trim() == "") { ShowWarning("Vui lòng nhập chẩn đoán."); return; }
            try
            {
                DatabaseHelper.ExecuteNonQuery(@"UPDATE BENHAN SET TrieuChung=@tc, ChanDoan=@cd, KetQuaKham=@kq, HuongDieuTri=@hdt WHERE MaBA=@ma",
                    DatabaseHelper.Param("@tc", txtTrieuChung.Text.Trim()), DatabaseHelper.Param("@cd", txtChanDoan.Text.Trim()),
                    DatabaseHelper.Param("@kq", txtKetQua.Text.Trim()), DatabaseHelper.Param("@hdt", txtHuong.Text.Trim()), DatabaseHelper.Param("@ma", txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật bệnh án.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// del
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn một dòng bệnh án để xóa."); return; }
            if (!Ask("Xóa bệnh án " + txtMa.Text + "?")) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BENHAN WHERE MaBA=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                LoadData();
                ClearInputs();
            }
            catch (Exception ex) { ShowError(ex, "Bệnh án này đã có đơn thuốc nên không thể xóa."); }
        }
    }
}
