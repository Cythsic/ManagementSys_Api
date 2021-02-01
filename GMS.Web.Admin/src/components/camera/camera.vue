<template>
    <div id="Photograph">
        <div class="ptoContainer">
            <video id="video" class="video" width="320" autoplay="autoplay"></video>
            <div
                :style="{backgroundImage: `url(${image})`}"
                class="headImage"
                v-if="image"
                v-show="!cameraStatus"
            ></div>
            <!-- 图片获取状态 -->
            <div class="photoViewStatus" v-show="photoAlertStatus || statusInfo.status !== ''">
                <div class="Loading">
                    <img
                        :src="loading"
                        class="loader"
                        v-show="statusInfo.status === 'loading'"
                    />
                    <img
                        :src="success"
                        class="loaderNoAni"
                        v-show="statusInfo.status === 'success'"
                    />
                </div>
                <p>{{statusInfo.msg}}</p>
            </div>
        </div>
        <div class="btnBox">
            <!-- 拍照按钮 -->
            <button class="btn" v-if="!cameraStatus" @click="connectCameraFn">拍照</button>
            <button class="btn" v-else @click="takePhotoFn">确认</button>
        </div>
    </div>
</template>

<script>
import { judgeBrowser, convertBase64UrlToBlob } from '../common/util'
import success from '@/assets/images/logo.jpg'
import loading from '@/assets/images/logo-min.jpg'

export default {
  name: '',
  props: {
    image: {
      type: String,
      default: ''
    },
    statusInfo: {
      type: Object,
      default: {
        status: '',
        msg: ''
      }
    }
  },
  data () {
    return {
      loading,
	  success,
      cameraStatus: false, // 拍照状态， true => 开启；false => 关闭
      photo: '',
      photoAlertStatus: false, // 图像提示文案显示状态 true => 显示；false => 隐藏
      getStatus: true // true => loading状态, false => 失败状态
    }
  },
  computed: {
    // 浏览器类型/版本(控制摄像头video区域大小)，在部分浏览器获取的摄像头拍照区域为长方形，所以需要判断浏览器，当前低版本的 safari 和 uc 会是长方形区域
    systemCameraVideo () {
      let system = judgeBrowser()
      let v = parseInt(system.version.replace(/\./g, ''))
      if (
        system.browser === 'chrome' ||
                (system.browser === 'safari' && v >= 1211)
      ) {
        return true
      }
      return false
    }
  },
  methods: {
    // 连接摄像头
    connectCameraFn () {
      this.photoAlertStatus = false // 关闭提示框
      // 区域控制
      let obj = {}
      if (this.systemCameraVideo) {
        obj = { width: 320, height: 320 }
      } else {
        obj = { width: 320 }
      }
      // end
      let constraints = {
        video: obj,
        audio: false
      }
      const video = document.getElementById('video')
      // 当前在IE浏览器无法调用摄像头
      try {
        let Promise = navigator.mediaDevices.getUserMedia(constraints)
        Promise.then(MediaStream => {
          video.srcObject = MediaStream
          window.MediaStreamTrack = MediaStream.getTracks()[0] // 摄像头对象
          video.play()
          this.cameraStatus = true
        })
      } catch (err) {
        this.$toast(
          '浏览器不支持调用摄像头，请更换最新的谷歌浏览器，或者其他最新版',
          3000,
          'upFade'
        )
      }
    },
    // 绘制照片
    takePhotoFn () {
      let video = document.getElementById('video')
      let canvas = document.createElement('canvas')
      canvas.width = canvas.height = 320
      let ctx = canvas.getContext('2d')
      let w = video.offsetWidth
      let h = video.offsetHeight
      let y = (canvas.width - h) / 2
      // 区域控制
      if (this.systemCameraVideo) {
        y = 0
      }
      ctx.drawImage(video, 0, y, w, h)
      let base64 = canvas.toDataURL('image/jpeg', 0.4)
      this.photo = base64
      // base64 => FormData
      let Blob = convertBase64UrlToBlob(base64)
      let formData = new FormData()
      formData.append('file', Blob)
      this.$emit('getPhoto', { formData: this.photo, base64: base64 })

      console.log(base64)
      // 关闭
      this.cameraStatus = false // 修改摄像头启动状态
      window.MediaStreamTrack &&
                window.MediaStreamTrack.stop &&
                window.MediaStreamTrack.stop() // 关闭摄像头
      this.deviceInfoStatus = false
    }
  }
}
</script>
