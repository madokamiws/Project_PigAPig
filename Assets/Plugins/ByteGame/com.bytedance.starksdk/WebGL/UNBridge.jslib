mergeInto(LibraryManager.library,{
    unityCallJs:function(msg){
        if (typeof UTF8ToString !== "undefined") {
            UNBridgeCore.handleMsgFromUnity(UTF8ToString(msg));
        } else {
            UNBridgeCore.handleMsgFromUnity(Pointer_stringify(msg));
        }
    },
    unityCallJsSync:function(msg){
        var result;
        if (typeof UTF8ToString !== "undefined") {
            result = UNBridgeCore.handleMsgFromUnitySync(UTF8ToString(msg));
        } else {
            result = UNBridgeCore.handleMsgFromUnitySync(Pointer_stringify(msg));
        }
        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);
        return buffer;
    },
    h5HasAPI:function(apiName){
        if (typeof UTF8ToString !== "undefined") {
            return UNBridge.h5HasAPI(UTF8ToString(apiName));
        } else {
            return UNBridge.h5HasAPI(Pointer_stringify(apiName));
        }
    },
    unityMixCallJs:function(msg){
        var result;
        if (typeof UTF8ToString !== "undefined") {
            result = UNBridgeCore.onUnityMixCall(UTF8ToString(msg));
        } else {
            result = UNBridgeCore.onUnityMixCall(Pointer_stringify(msg));
        }
        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);
        return buffer;
    }
});
