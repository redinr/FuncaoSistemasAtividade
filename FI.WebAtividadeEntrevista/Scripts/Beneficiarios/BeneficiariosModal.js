const beneficiarios = [];

$(document).ready(function () {
    FormataCPF();
})

function incluirBeneficiario() {
    const cpfBeneficiario = document.getElementById('cpfBeneficiario').value;
    const nomeBeneficiario = document.getElementById('nomeBeneficiario').value;


    var url = null;
    if (typeof urlIncluirBeneficiario !== 'undefined') {
        url = urlIncluirBeneficiario;
    } else {
        url = urlAlterarBeneficiario;
    }

    if (cpfBeneficiario && nomeBeneficiario) {
        $.ajax({
            url: url,
            method: "POST",
            data: {
                "CPF": cpfBeneficiario,
                "Nome": nomeBeneficiario
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                }

        });

        beneficiarios.push({ cpfBeneficiario, nomeBeneficiario });
        renderBeneficiarios();
        document.getElementById('beneficiarioForm').reset();
    }
}

function renderBeneficiarios() {
    const beneficiariosList = document.getElementById('beneficiariosList');
    beneficiariosList.innerHTML = '';

    beneficiarios.forEach((beneficiario, index) => {
        const row = `<tr>
                  <td>${beneficiario.cpfBeneficiario}</td>
                  <td>${beneficiario.nomeBeneficiario}</td>
                  <td>
                    <button class="btn btn-primary btn-sm" onclick="alterarBeneficiario(${index})">Alterar</button>
                    <button class="btn btn-danger btn-sm" onclick="excluirBeneficiario(${index})">Excluir</button>
                  </td>
                </tr>`;
        beneficiariosList.innerHTML += row;
    });
}

function alterarBeneficiario(index) {
    const beneficiario = beneficiarios[index];
    document.getElementById('cpfBeneficiario').value = beneficiario.cpfBeneficiario;
    document.getElementById('nomeBeneficiario').value = beneficiario.nomeBeneficiario;
    excluirBeneficiario(index); 
}

function excluirBeneficiario(index) {
    beneficiarios.splice(index, 1);
    renderBeneficiarios();
}

function FormataCPF() {
    $('#cpfBeneficiario').on('input', function () {
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