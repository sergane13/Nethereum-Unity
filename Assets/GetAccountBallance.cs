using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Signer;


[Function("transfer", "bool")]
public class TransferFunction : FunctionMessage
{
    [Parameter("address", "_to", 1)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 2)]
    public BigInteger TokenAmount { get; set; }
}


[Function("balanceOf", "uint256")]
public class BalanceOfFunction : FunctionMessage
{
    [Parameter("address", "_owner", 1)]
    public string Owner { get; set; }
}


public class GetAccountBallance : MonoBehaviour
{
    [SerializeField] private InputField _addressField;
    [SerializeField] private InputField _amount;
    [SerializeField] private Button _buttonSendTx;
    [SerializeField] private Text _blockConfirmed;

    private Web3 myAccount;

    // Use this for initialization
    void Start()
    {
        myAccount = SetAccount();
        _buttonSendTx.onClick.AddListener(GetBlockNumber);
    }


    private Web3 SetAccount()
    {
        var url = "-node provider-";
        var privateKey = "-- private key--";
        var account = new Account(privateKey, 80001);
        var web3 = new Web3(account, url);

        web3.TransactionManager.UseLegacyAsDefault = true;

        return web3;
    }


    public async void GetBlockNumber()
    {
        var transferHandler = myAccount.Eth.GetContractTransactionHandler<TransferFunction>();
        var transfer = new TransferFunction()
        {
            To = _addressField.text,
            TokenAmount = Int32.Parse(_amount.text) * (BigInteger)Math.Pow(10, 18) //wei
        };

        var contractAddress = "0x4c4ac1d786fcd6d9ad25f3c2fa540ab56d48def6";
        var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);

        _blockConfirmed.text = transactionReceipt.BlockNumber.ToString();
    }


    private void OnDestroy()
    {
        _buttonSendTx.onClick.RemoveAllListeners();
    }
}




    //var web3 = new Web3("-- node provider --");

    //var balance = await web3.Eth.GetBalance.SendRequestAsync("0x22cFDCba61311a188A238D5C3F4fd6D7bEC2EccC");

    //var balanceInWei = balance.Value; ;
    //var balanceInEther = Web3.Convert.FromWei(balanceInWei);

    //print("MyBalance: " + balanceInEther);