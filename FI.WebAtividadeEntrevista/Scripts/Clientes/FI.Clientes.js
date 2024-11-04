
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

        var beneficiariosData = [];
        const beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
        beneficiariosTemp.forEach(f => {
            if (f.Id === 0 && f.IdCliente === 0) {
                beneficiariosData.push(f);
            }
        });

        if (!validaCpf(clienteData.CPF)) {
            ModalDialog("Ocorreu um erro", "O CPF informado é inválido.");
            return;
        }

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

//Formata o cpf conforme for digitando.
function formataCPF() {
    $('#CPF').on('input', function () {
        let cpf = $(this).val().replace(/\D/g, '');

        if (cpf.length > 9) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        } else if (cpf.length > 6) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{1,3})/, "$1.$2.$3");
        } else if (cpf.length > 3) {
            cpf = cpf.replace(/(\d{3})(\d{1,3})/, "$1.$2");
        }

        $(this).val(cpf);
    });
}

//Verifica se o cpf é válido.
function validaCpf(cpf) {
    cpf = cpf.replace(/\D/g, "");

    if (cpf.length !== 11 || /^(\d)\1{10}$/.test(cpf)) {
        return false;
    }

    let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseInt(cpf.charAt(i)) * (10 - i);
    }

    let resto = 11 - (soma % 11);
    let digito1 = resto === 10 || resto === 11 ? 0 : resto;

    if (digito1 !== parseInt(cpf.charAt(9))) {
        return false;
    }

    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpf.charAt(i)) * (11 - i);
    }

    resto = 11 - (soma % 11);
    let digito2 = resto === 10 || resto === 11 ? 0 : resto;

    return digito2 === parseInt(cpf.charAt(10));
}