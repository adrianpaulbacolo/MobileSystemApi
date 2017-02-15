<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="PaySec2.aspx.cs" Inherits="Deposit_PaySec2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style='margin: auto; width: 1070px; padding: 10px;'>
        <div style='height: 130px; width: 50%; padding-top: 48px; float: left; margin: 5px;'>
            <ul class='tab'>
                <li><a href='javascript:void(0)' class='tablinks' id='atmLink' onclick='openTab(event, ""atm"")'>ATM</a></li>
                <li><a href='javascript:void(0)' class='tablinks' id='bankingLink' onclick='openTab(event, ""internetbangking"")'>Internet Banking</a></li>
            </ul>
            <div id='atm' class='tabcontent'>
                <button class='accordion' onclick='openAccordion(event, ""bca"")'>BCA</button>
                <div id='bca' class='panel'>
                    <ol>
                        <li class='steps'>Setelah Anda Berhasil masuk ke dalam akun BCA Anda, pilih menu <b>Transaksi Lainnya</b>.</li>
                        <li class='steps'>Pilih menu <b>Transfer</b>, lalu <b>Ke Rek, Bank Lain</b>.</li>
                        <li class='steps'>Masukkan Kode Sandi Bank 016 (Maybank) dan pilih tombol <b>Benar</b></li>
                        <li class='steps'>Masukkan “<b>7893 **** **** ****</b>” sebagai Nomor Rekening Tujuan dan pilih tombol <b>Benar</b>.(Maybank) dan pilih tombol <b>Benar</b>.</li>
                        <li class='steps'>Masukkan jumlah transfer <b>*Amount*</b> dan pilih tombol Benar.</li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layar mesin ATM sudah benar lalu pilih tombol <b>Benar</b> untuk lanjut.</li>
                        <li class='steps'>Mesin ATM akan mengeluarkan struk ATM sebagai bukti pembayaran. Transaksi Anda otomatis akan diproses.
					        <p>*Jika Anda harus mengetik jumlah transfer di mesin ATM secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank. </p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""bankmandiri"")'>Bank Mandiri</button>
                <div id='bankmandiri' class='panel'>
                    <ol>
                        <li class='steps'>Setelah Anda Berhasil masuk ke dalam akun Bank Mandiri Anda, pilih menu <b>Transaksi Lainnya</b>.</li>
                        <li class='steps'>Pilih menu <b>Transfer</b>, lalu <b>Antar Bank Online</b>.</li>
                        <li class='steps'>Masukkan Kode Sandi Bank 016 (Maybank) dan pilih tombol <b>Benar</b></li>
                        <li class='steps'>Masukkan “<b>7893 **** **** ****</b>” sebagai Nomor Rekening Tujuan dan pilih Tombol <b>Benar</b>.</li>
                        <li class='steps'>Masukkan jumlah transfer <b>*Amount*</b> dan pilih tombol <b>Benar</b>.</li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer mesin ATM sudah benar lalu pilih tombol <b>Ya</b> untuk lanjut.</li>
                        <li class='steps'>Mesin ATM akan mengeluarkan struk ATM sebagai bukti pembayaran. Transaksi Anda otomatis akan diproses.
					        <p>*Jika Anda harus mengetik jumlah transfer di mesin ATM secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""bni"")'>BNI Bank Negara Indonesia</button>
                <div id='bni' class='panel'>
                    <ol>
                        <li class='steps'>Setelah Anda Berhasil masuk ke dalam akun BNI Anda, pilih <b>Menu Lain.</b></li>
                        <li class='steps'>Pilih menu <b>Transfer</b>, lalu <b>Ke Rek, Bank Lain</b>.</li>
                        <li class='steps'>Masukkan Kode Sandi Bank 016 (Maybank) diikuti dengan “<b>7893 **** **** ****</b>”, lalu pili tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Masukkan jumlah transfer <b>*Amount*</b> dan pilih tombol Tekan Jika Benar</li>
                        <li class='steps'>Masukkan nomor referensi (boleh dikosongkan) dan pilih tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer mesin ATM sudah benar lalu pilih tombol <b>Tekan Jika Benar</b> untuk lanjut.</li>
                        <li class='steps'>Mesin ATM akan mengeluarkan struk ATM sebagai bukti pembayaran. Transaksi Anda otomatis akan diproses.
					        <p>*Jika Anda harus mengetik jumlah transfer di mesin ATM secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""permata"")'>Permata Bank</button>
                <div id='permata' class='panel'>
                    <ol>
                        <li class='steps'>Setelah Anda Berhasil masuk ke dalam akun Bank Permata Anda, pilih menu <b>Transaksi Lainnya</b></li>
                        <li class='steps'>Pili men <b>Transaksi Pembayaran</b>, lalu <b>Lain-lain</b>, lalu <b>Pembayaran Virtual Account</b>.</li>
                        <li class='steps'>Masukkan Kode Bank 016 (Maybank) diikuti dengan “<b>7893 **** **** ****</b>”, lalu pili tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Masukkan jumlah transfer <b>*Amount*</b> dan pilih tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer mesin ATM sudah benar lalu pilih tombol <b>Benar</b> untuk lanjut.</li>
                        <li class='steps'>Mesin ATM akan mengeluarkan struk ATM sebagai bukti pembayaran. Transaksi Anda otomatis akan diproses.
					        <p>*Jika Anda harus mengetik jumlah transfer di mesin ATM secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""cimb"")'>CIMB Bank</button>
                <div id='cimb' class='panel'>
                    <ol>
                        <li class='steps'>Setelah Anda Berhasil masuk ke dalam akun Bank CIMB Niaga Anda, pilih menu <b>Menu Lain</b>.</li>
                        <li class='steps'>Pilih menu <b>Transfer</b>, lalu <b>Ke Rek, Bank Lain</b>.</li>
                        <li class='steps'>Masukkan Kode Sandi Bank 016 (Maybank) dan lalu pili tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Masukkan “<b>7893 **** **** ****</b>” sebagai Nomor Rekening Tujuan dan pilih Tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Masukkan jumlah transfer <b>*Amount*</b> dan pilih tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Masukkan nomor referensi (boleh dikosongkan) dan pilih tombol <b>Tekan Jika Benar</b></li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer mesin ATM sudah benar lalu pilih tombol <b>Tekan Jika Benar</b> untuk lanjut.</li>
                        <li class='steps'>Mesin ATM akan mengeluarkan struk ATM sebagai bukti pembayaran. Transaksi Anda otomatis akan diproses.
					        <p>*Jika Anda harus mengetik jumlah transfer di mesin ATM secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
            </div>
            <div id='internetbangking' class='tabcontent'>
                <button class='accordion' onclick='openAccordion(event, ""bmibp"")'>Bank Mandiri - Internet Banking Payment</button>
                <div id='bmibp' class='panel'>
                    <ol>
                        <li class='steps'>Login ke dalam akun <b>Mandiri Internet</b> Anda.</li>
                        <li class='steps'>Pilih menu <b>Transfer, Antar Bank Online</b>.</li>
                        <li class='steps'>Pilih rekening Anda dari kolom Dari Rekening dan ketik <b>*Amount*</b> di Jumlah Transfer</li>
                        <li class='steps'>Klik tombol <b>Rekening</b> Penerima dan pilih bank (016) Maybank dari Nama Bank.</li>
                        <li class='steps'>Ketik “<b>7893 **** **** ****</b>” di kolom Nomor Rekening dan klik tombol <b>Lanjutkan</b>.</li>
                        <li class='steps'>Perkisa data yang ditamplikan di layer sudah benar lalu lanjutkan transaksi dengan token PIN Mandiri Anda dan klik tombol <b>Lanjutkan</b>.</li>
                        <li class='steps'>Anda akan dapat melihat bukti transfer Anda telah berhasil.
					        <p>*Jika Anda harus mengetik jumlah transfer di internet banking secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""cimbibp"")'>CIMB Bank - Internet Banking Payment</button>
                <div id='cimbibp' class='panel'>
                    <ol>
                        <li class='steps'>Setelah login ke dalam akun <b>CIMB Clicks</b> Anda, pilih menu <b>Transfer</b>.</li>
                        <li class='steps'>Ketik <b>*Amount*</b> di jumlah Transfer, pastikan mata uang yang dipilih adalah IDR.</li>
                        <li class='steps'>Pilih <b>Transfer ke Rekening Lain</b>, pilih bank (016) Maybank dari daftar yang ada, lalu klik tombol Lanjut.</li>
                        <li class='steps'>Ketiks “<b>7893 **** **** ****</b>” di kolom Rekening Penerima dan klik tombol <b>Lanjut</b> Kolom email boleh dikosongkan.</li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer sudah benar lalu masukkan mPin Anda dan klik tombol <b>Kirim</b> untuk lanjut.</li>
                        <li class='steps'>Anda akan dapat melihat bukti transfer Anda telah berhasil.
					        <p>*Jika Anda harus mengetik jumlah transfer di internet banking secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
                <button class='accordion' onclick='openAccordion(event, ""bniibp"")'>Bank Negara Indonesi (BNI) - Internet Banking Payment</button>
                <div id='bniibp' class='panel'>
                    <ol>
                        <li class='steps'>Login ke dalam akun <b>BNI Internet Banking</b> Anda.</li>
                        <li class='steps'>Pilih menu <b>TRANSAKSI, INFO &amp; Administrasi Transfer, Atur Rekening Tujuan</b>.</li>
                        <li class='steps'>Pili <b>Tambah Rekening Tujuan</b> dan klik tombol OK.</li>
                        <li class='steps'>Ketik naman referensi untuk transaksi ini (bebas semau Anda) di dalam kolom Nama Singkat.</li>
                        <li class='steps'>Pili <b>Transfer Online Antar Bank</b> klik tombol <b>Cari</b>. Cari (016) Maybank Inonesia TBK dari daftar bank yang ada dan klik tombol <b>Pilih</b>.</li>
                        <li class='steps'>Ketik “<b>7893 **** **** ****</b>” di kolom Nomor Rekening dan klik tombol <b>Lanjut</b>. Kolom lainnya akan otomatis terisi.</li>
                        <li class='steps'>Ketik ulang “<b>7893 **** **** ****</b>” di kolom Konfirmasi Nomor Rekening, pilih mata uang IDR, dan klik tombol <b>Lanjutkan</b>.</li>
                        <li class='steps'>Periksa data yang ditamplikan di layer sudah benar lalu lanjutkan transaksi dengan token BNI E-Secure Anda dan klik tombol <b>Proses</b>.</li>
                        <li class='steps'>Setelah berhasil menambahkan akun, pilih menu <b>Transfer, Transfer Online Antar Bank</b>.</li>
                        <li class='steps'>Pada DEtil Transaksi, pilih <b>Rekening</b> milik Anda.</li>
                        <li class='steps'>Pada detil Tujuan, pilih <b>Nama Singkat</b> yang baru saja Anda buat dan ketik <b>*Amount*</b> di kolom Jumlah, lalu klik tombol <b>Lanjutkan</b>. Kolom Nomor Referensi Nasabah boleh dikosongkan.</li>
                        <li class='steps'>Periksa data transaksi Anda yang ditampilkan di layer sudah benar lalu lanjutkan transaksi dengan token BNI E-secure Anda dan klik tombol <b>Proses</b>.</li>
                        <li class='steps'>Anda akan dapat melihat bukti transfer Anda telah berhasil.
					        <p>*Anda akan perlu mengulang langkah-langkah di atas untuk setiap transaksi.</p>
                            <p>*Jika Anda harus mengetik jumlah transfer di internet banking secara manual, ketik jumlah sebenarnya yang Anda ingin depositkan.</p>
                            <p>*Transaksi ini akan dikenakan biaya sesuai dengan ketentuan tarif transfer antar bank.</p>
                        </li>
                    </ol>
                </div>
            </div>
        </div>
        <div style='height: 130px; width: 48%; padding-top: 50px; float: left; margin: 5px; text-align: center'>
            <div style='width: 70%; margin: 0 auto;'>
                <div>
                    <p style='font: 35px Arial; font-weight: bold; margin: 0;' id="vaNumber"></p>
                    <p style='margin: 0;'>Nomor VA</p>
                </div>
                <div style='margin-top: 30px;'>
                    <p style='font: 35px Arial; font-weight: bold; margin: 0;' id="amount"></p>
                    <p style='margin: 0;'>Jumlah</p>
                </div>
                <div style='margin-top: 30px;'>
                    <p>Order ID: <span id="orderId"></span></p>
                    <p>Cart ID: <span id="cardId"></span></p>
                </div>
                <div style='border-style: solid;'>
                    <p>Silakan menyelesaikan transaksi ini sebelum:</p>
                    <p style='font: 18px Arial; font-weight: bold' id="vaExpiry"></p>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {

            var gateway = new w88Mobile.Gateway();
            var params = gateway.getUrlVars();

            $('#vaNumber').text(params.VANumber)
            $('#amount').text(params.Amount + " RP")
            $('#orderId').text(params.OrderId)
            $('#cardId').text(params.CartId)
            $('#vaExpiry').text(params.VAExpiry)


            document.getElementById('atm').style.display = 'block';
            document.getElementById('atmLink').className += ' active';

            function openAccordion(evt, accordionName) {
                // Declare all variables
                var i, tabcontent, tablinks;

                // Get all elements with class="tabcontent" and hide them
                tabcontent = document.getElementsByClassName('panel');
                for (i = 0; i < tabcontent.length; i++) {
                    tabcontent[i].style.display = 'none';
                }

                // Get all elements with class="tablinks" and remove the class "active"
                tablinks = document.getElementsByClassName('accordion');
                for (i = 0; i < tablinks.length; i++) {
                    tablinks[i].className = tablinks[i].className.replace(' active', '');
                }

                // Show the current tab, and add an "active" class to the link that opened the tab
                document.getElementById(accordionName).style.display = 'block';
                evt.currentTarget.className += ' active';

                document.getElementById(accordionName).style.maxHeight = document.getElementById(accordionName).scrollHeight + 'px';

            }

            function openTab(evt, tabName) {
                // Declare all variables
                var i, tabcontent, tablinks;

                // Get all elements with class="tabcontent" and hide them
                tabcontent = document.getElementsByClassName('tabcontent');
                for (i = 0; i < tabcontent.length; i++) {
                    tabcontent[i].style.display = 'none';
                }

                // Get all elements with class="tablinks" and remove the class "active"
                tablinks = document.getElementsByClassName('tablinks');
                for (i = 0; i < tablinks.length; i++) {
                    tablinks[i].className = tablinks[i].className.replace(' active', '');
                }

                // Show the current tab, and add an "active" class to the link that opened the tab
                document.getElementById(tabName).style.display = 'block';
                evt.currentTarget.className += ' active';
            }

        });
    </script>
</asp:Content>
