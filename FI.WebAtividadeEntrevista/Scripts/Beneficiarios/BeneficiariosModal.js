const beneficiarios = [];
var IdCliente = 0;

$(document).ready(function () {
    $('#beneficiariosModal').on('show.bs.modal', function () {
        IdCliente = Number($('#formCadastro #fieldIdCliente').val());
        listarBeneficiarios();
    });
})

function incluirBeneficiario() {
    const Id = document.getElementById('fieldId').value;;
    const CPF = document.getElementById('cpfBeneficiario').value;
    const Nome = document.getElementById('nomeBeneficiario').value;

    if (CPF && Nome) {
        let beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
        beneficiariosTemp.push({ Id, CPF, Nome, IdCliente });

        sessionStorage.setItem('beneficiariosTemp', JSON.stringify(beneficiariosTemp));

        $('#beneficiariosList').empty();
        document.getElementById('beneficiarioForm').reset();
        listarBeneficiarios();
    }
}

function listarBeneficiarios() {

    const beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];

    if (beneficiariosTemp.length !== 0) {
        preencheGrid(beneficiariosTemp);
    } else {
        $.ajax({
            url: '/Beneficiarios/GetListaBeneficiarios',
            method: 'GET',
            success: function (data) {

                data.forEach(f => {
                    beneficiariosTemp.push({ Id: f.Id, CPF: f.CPF, Nome: f.Nome, IdCliente: f.IdCliente });

                    sessionStorage.setItem('beneficiariosTemp', JSON.stringify(beneficiariosTemp));
                });

                preencheGrid(data);
            },
            error: function () {
                ModalDialog("Ocorreu um erro", "Erro ao carregar a lista de beneficiários.");
            }
        });
    }
}

function preencheGrid(listaDados) {
    const beneficiariosList = $('#beneficiariosList');
    beneficiariosList.empty();

    var index = 0;
    listaDados.forEach(beneficiario => {
        if (IdCliente == beneficiario.IdCliente) {
            var id = beneficiario.Id;
            var cpf = formatarCPF(beneficiario.CPF);
            var nome = beneficiario.Nome;
            var idCliente = beneficiario.IdCliente;

            const row = `
                        <tr>
                            <td style="display: none">${id}</td>
                            <td>${cpf}</td>
                            <td>${nome}</td>
                            <td style="display: none">${idCliente}</td>
                            <td>
                                <button type="button" class="btn btn-primary btn-sm alterar-btn" onclick="alterarBeneficiario(${index})">Alterar</button>
                                <button type="button" class="btn btn-primary btn-sm deletar-btn" onclick="excluirBeneficiario(${index})">Excluir</button>
                            </td>
                        </tr>`;
            beneficiariosList.append(row);
            index++;
            beneficiarios.push({ Id: id, CPF: cpf, Nome: nome, IdCliente: idCliente });
        }
    });
}

function alterarBeneficiario(index) {
    const beneficiario = beneficiarios[index];
    document.getElementById('fieldId').value = beneficiario.Id;
    document.getElementById('cpfBeneficiario').value = beneficiario.CPF;
    document.getElementById('nomeBeneficiario').value = beneficiario.Nome;
    document.getElementById('fieldIdCliente').value = beneficiario.IdCliente;
    excluirBeneficiario(index);
}

function excluirBeneficiario(index) {
    let beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
    beneficiariosTemp.splice(index, 1);
    sessionStorage.setItem('beneficiariosTemp', JSON.stringify(beneficiariosTemp));
    beneficiarios.splice(index, 1);      
    listarBeneficiarios();
}

function formatarCPF(cpf) {
    cpf = cpf.replace(/\D/g, "");
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
    cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    return cpf;
}