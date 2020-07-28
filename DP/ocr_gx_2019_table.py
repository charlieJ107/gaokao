from tencentcloud.common import credential
from tencentcloud.common.profile.client_profile import ClientProfile
from tencentcloud.common.profile.http_profile import HttpProfile
from tencentcloud.common.exception.tencent_cloud_sdk_exception import TencentCloudSDKException
from tencentcloud.ocr.v20181119 import ocr_client, models
import base64

apiKey=""
apiSecret=""

for i in [195,660,964]:
    imgUrl = "https://data-vankyle-1257862518.cos.ap-shanghai.myqcloud.com/image/gaokao/guangxi-2020/zhinan/jpg/guangxi-2020_" + str(i) + ".jpg"
    jsonFileName = "guangxi-2020_" + str(i) + ".json"
    ExcelFileName = "guangxi-2020_"+str(i)+".xlsx"
    print(imgUrl)
    print(jsonFileName)
    try:
        cred=credential.Credential(apiKey, apiSecret)
        httpProfile = HttpProfile()
        httpProfile.endpoint = "ocr.ap-shanghai.tencentcloudapi.com"

        clientProfile = ClientProfile()
        clientProfile.httpProfile = httpProfile
        client = ocr_client.OcrClient(cred, "ap-shanghai", clientProfile) 

        req = models.TableOCRRequest()
        params = '{\"ImageUrl\":\"'+imgUrl+'\"}'
        req.from_json_string(params)

        resp = client.TableOCR(req)
        with open("./ocr/guangxi-2019/zhinan/json/" + jsonFileName, 'w', encoding="utf-8") as jsonFile:
            jsonFile.write(resp.to_json_string())

        with open("./ocr/guangxi-2019/zhinan/xlsx/" + ExcelFileName, 'wb') as ExcelFile:
            ExcelFile.write(base64.b64decode(resp.Data))

    except TencentCloudSDKException as err: 
        with open("./err.log", 'a', encoding="utf-8") as log:
            log.write("=================="+str(i)+"==========================\n")
            log.write(imgUrl+'\n')
            log.write(jsonFileName+'\n')
            log.write(str(err)+'\n')         
            log.write("======================================================\n")
        print(err)

