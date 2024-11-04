
$(document).ready(function () {
    formataCPF();
    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        const clienteData = {
            NOME: $('#formCadastro').find("#Nome").val(),
            CEP: $('#formCadastro').find("#CEP").val(),
            Email: $('#formCadastro').find("#Email").val(),
            Sobrenome: $('#formCadastro').find("#Sobrenome").val(),
            Nacionalidade: $('#formCadastro').find("#Nacionalidade").val(),
            CPF: $('#formCadastro').find("#CPF").val(),
            Estado: $('#formCadastro').find("#Estado").val(),
            Cidade: $('#formCadastro').find("#Cidade").val(),
            Logradouro: $('#formCadastro').find("#Logradouro").val(),
            Telefone: $('#formCadastro').find("#Telefone").val()
        };

        const beneficiariosData = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];

        $.ajax({
            url: urlPost,
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify({ cliente: clienteData, beneficiarios: beneficiariosData }),
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r);
                    sessionStorage.removeItem('beneficiariosTemp');
                    $("#formCadastro")[0].reset();
                }
        });
    })

})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function formataCPF() {
    $('#CPF').on('input', function () {
        let cpf = $(this).val().replace(/\D/g, ''); // Remove qualquer caractere não numérico

        if (cpf.length > 9) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        } else if (cpf.length > 6) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{1,3})/, "$1.$2.$3");
        } else if (cpf.length > 3) {
            cpf = cpf.replace(/(\d{3})(\d{1,3})/, "$1.$2");
        }

        $(this).val(cpf); // Atualiza o campo com o CPF formatado
    });
}