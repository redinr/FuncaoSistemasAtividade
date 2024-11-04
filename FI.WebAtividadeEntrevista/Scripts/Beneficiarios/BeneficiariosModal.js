var IdCliente = 0;

$(document).ready(function () {
    $('#beneficiariosModal').on('show.bs.modal', function () {
        IdCliente = Number($('#formCadastro #fieldIdCliente').val());
        listarBeneficiarios();
    });
})

// Inclui o novo beneficiario na lista temporária.
function incluirBeneficiario() {
    var Id = document.getElementById('fieldId').value;
    var CPF = document.getElementById('cpfBeneficiario').value;
    const Nome = document.getElementById('nomeBeneficiario').value;

    if (typeof Id === 'undefined') {
        Id = 0;
    }

    CPF = CPF.replace(/\D/g, "");

    if (!validaCpf(CPF)) {
        ModalDialog("Ocorreu um erro", "O CPF informado é inválido.");
        return;
    }

    if (verificaDuplicidadeCpf(CPF)) {
        ModalDialog("Ocorreu um erro", "Já existe um beneficiário com este CPF.");
        return;
    }

    if (CPF && Nome) {
        let beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
        beneficiariosTemp.push({ Id, CPF, Nome, IdCliente });

        sessionStorage.setItem('beneficiariosTemp', JSON.stringify(beneficiariosTemp));

        $('#beneficiariosList').empty();
        document.getElementById('beneficiarioForm').reset();
        listarBeneficiarios();
    }
}

// Busca na lista ou no banco de dados os beneficiarios.
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

// Preenche a grid da pop-up com os dados.
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
        }
        index++;
    });
}

// Carrega os dados para alterar as informacoes do beneficiario.
function alterarBeneficiario(index) {
    var beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || []
    const beneficiario = beneficiariosTemp[index];
    document.getElementById('fieldId').value = beneficiario.Id;
    document.getElementById('cpfBeneficiario').value = beneficiario.CPF;
    document.getElementById('nomeBeneficiario').value = beneficiario.Nome;
    document.getElementById('fieldIdCliente').value = beneficiario.IdCliente;
    excluirBeneficiario(index);
}

// Exclui um beneficiario da lista.
function excluirBeneficiario(index) {
    let beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
    beneficiariosTemp.splice(index, 1);
    sessionStorage.setItem('beneficiariosTemp', JSON.stringify(beneficiariosTemp));
    listarBeneficiarios();
}

// Formata o cpf antes de exibir na tela.
function formatarCPF(cpf) {
    cpf = cpf.replace(/\D/g, "");
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
    cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    return cpf;
}

// Verifica se o cpf é válido.
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

//Verifica se já existe um cpf na lista.
function verificaDuplicidadeCpf(cpf) {
    let beneficiariosTemp = JSON.parse(sessionStorage.getItem('beneficiariosTemp')) || [];
    return beneficiariosTemp.some(b => b.CPF === cpf);
}
