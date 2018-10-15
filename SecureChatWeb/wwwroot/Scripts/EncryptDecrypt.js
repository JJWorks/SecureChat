var mypubKey, myprivKey;

GenerateKey(document.getElementById("inputPassword").value, "bobthebuilder");



async function GenerateKey(passwordtouse, username) {
    var options = {
        userIds: [{ name: username, email: username.replace(/ /g, '') + '@example.com' }],
        passphrase: passwordtouse
    }
 
    await openpgp.generateKey(options).then(key => {
        myprivKey = key.privateKeyArmored
        mypubKey = key.publicKeyArmored
        console.log('Key generated')
    })
}

function decrypt(privKey, passwordtouse, message) {
    alert(mypubKey);
    var key = openpgp.key.readArmored(privKey).keys[0]
    key.decrypt(passwordtouse)
    var options = {
        message: openpgp.message.readArmored(message),
        privateKey: key
    }
    return openpgp.decrypt(options).then(decryptedMessage => {
        return decryptedMessage.data;
    })
}


async function encryptWithMultiplePublicKeys(pubkeys, privkey, passphrase, message) {
    const privKeyObj = (await openpgp.key.readArmored(privkey)).keys[0]
    await privKeyObj.decrypt(passphrase)

    pubkeys = pubkeys.map(async (key) => {
        return (await openpgp.key.readArmored(key)).keys[0]
    });

    const options = {
        message: openpgp.message.fromText(message),
        publicKeys: pubkeys,           				  
        privateKeys: privKeyObj
    }

    return openpgp.encrypt(options).then(ciphertext => {
        encrypted = ciphertext.data // '-----BEGIN PGP MESSAGE ... END PGP MESSAGE-----'
        return encrypted
    })
};

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}