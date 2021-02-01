<template>
  <div id="app">
    <div class="content"> <span style="color:red">base64数据：</span>{{image}}</div>
    <Photograph :image="image" :statusInfo="statusInfo" @getPhoto="getPhoto"></Photograph>
  </div>
</template>

<script>
import Photograph from '_c/camera'
export default {
  name: 'App',
  components: {
    Photograph
  },
  data () {
    return {
      image: '', // 传递图像数据
      statusInfo: { // 状态显示
        status: '', // loading | fail
        msg: ''
      }
    }
  },
  mounted () {},
  methods: {
    // 获取拍照图像
    getPhoto ({ formdata, base64 }) {
      // 上传中
      this.statusInfo.status = 'loading'
      this.statusInfo.msg = '上传中'
      // 上传失败
      setTimeout(() => {
        this.statusInfo.status = 'success'
        this.statusInfo.msg = '成功'
      }, 2000)
      // 上传成功
      setTimeout(() => {
        this.statusInfo.status = ''
        this.statusInfo.msg = ''
        this.image = base64
      }, 4000)
    }
  }
}
</script>
