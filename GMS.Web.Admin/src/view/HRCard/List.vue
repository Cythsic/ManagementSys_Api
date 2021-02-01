
<template>
  <div class="content-main">
    <div class="search-con search-con-top">
      <Form ref="formInline" label-position="right" :label-width="60" inline>
        <FormItem label="工号">
          <Input class="search-input" v-model="queryData.SL_UserId" />
        </FormItem>
        <FormItem label="电话">
          <Input class="search-input" v-model="queryData.SL_PhoneNumber" />
        </FormItem>
        <FormItem>
          <Button class="search-btn" type="primary" @click="setPageData(1)">
            <Icon type="search" />&nbsp;&nbsp;搜索
          </Button>
        </FormItem>
		<FormItem>
		  <Button class="search-btn" type="primary" @click="openSerialPort()">
		    <Icon type="search" />&nbsp;&nbsp;打开串口
		  </Button>
		</FormItem>
      </Form>
    </div>

    <Card>
      <div>
        <Button class="search-btn" type="success" size="small" style="margin-right: 5px;" @click="handleAdd">
          <Icon type="md-add" />&nbsp;&nbsp;新增
        </Button>
		<Upload style="display: inline-block" action="">
		  <Button type="primary" size="small" icon="ios-cloud-upload-outline">导入</Button>
		</Upload>
      </div>
      <Table ref="tables" :data="tableData1" v-bind:columns="tableColumns1" stripe></Table>
      <!-- <tables ref="tables" editable v-model="tableData1" :columns="tableColumns1" @on-delete="handleDelete" stripe /> -->
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page
            :total="pageTotal"
            :current="pageCurrent"
            @on-change="changePage"
            @on-page-size-change="changePageSize"
            show-total
            show-elevator
            show-sizer
          ></Page>
        </div>
      </div>
    </Card>

   <Modal
      title="编辑"
      :mask-closable="false"
      v-model="modelEdit"
      width="800"
      scrollable
      footer-hide
      @on-ok="saveEdit"
    >
      <Edit ref="edit" :parent="this" :edit-row="eidtRow"></Edit>
    </Modal>

	<Modal
	  title="拍照"
	  :mask-closable="false"
	  v-model="modelPhoto"
	  width="800"
	  scrollable
	  footer-hide
	>
	  <Photo ref="Photo" :parent="this" :edit-row="eidtRow"></Photo>
	</Modal>

  </div>
</template>
<script>
import '@/assets/css/util.less'
import Edit from './Edit'
import Photo from './Photo'
import { getPage, remove as removeUser } from '@/api/HRUser'

export default {
  name: 'userManage',
  components: {
    Edit,
    Photo
  },
  data () {
    return {
      tableData1: [],
      queryData: {},
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      modelPhoto: false,
      isAdd: true,
      eidtRow: {},
	  connect_toserver: '',
      tableColumns1: [
        {
          title: '编号',
          key: 'id'
        },
        {
          title: '工号',
          key: 'userId'
        },
        {
          title: '姓名',
          key: 'userName'
        },
        {
          title: '电话',
          key: 'phoneNumber'
        },
        {
		  title: '身份证过期时间',
		  key: 'expiryDate'
        },
        {
          title: '修改时间',
          key: 'modifyTime'
        },
        {
          title: '操作',
          key: 'action',
          width: 300,
          align: 'center',
          render: (h, params) => {
            return h('div', [
              h(
                'Button',
                {
                  props: {
                    type: 'primary',
                    size: 'small',
                    icon: 'md-create'
                  },
                  style: {
                    marginRight: '5px'
                  },
                  on: {
                    click: () => {
                      this.handleEdit(params)
                    }
                  }
                },
                '编辑'
              ),
              h(
                'Button',
                {
                  props: {
                    type: 'error',
                    size: 'small',
                    icon: 'md-trash'
                  },
				  style: {
				    marginRight: '5px'
				  },
                  on: {
                    click: () => {
                      this.handleDelete(params)
                    }
                  }
                },
                '删除'
              ),
			  h(
			    'Button',
			    {
			      props: {
			        type: 'warning',
			        size: 'small',
			        icon: 'md-trash'
			      },
			      on: {
			        click: () => {
			          this.handleTakePhoto(params)
			        }
			      }
			    },
			    '拍照'
			  )
            ])
          }
        }
      ]
    }
  },
  computed: {},
  methods: {
    setPageData (pageNum = this.pageCurrent, pageSize = 10) {
      getPage({
        pageNum: pageNum,
        pageSize: pageSize,
        field: 'Id',
        order: 'asc',
        query: this.queryData
      })
        .then(res => {
          const resData = res.data
          const code = resData.code
          const msg = resData.msg
          if (code != 1) {
            this.$Message.error(code.msg)
            return
          }
          const data = resData.data
          this.tableData1 = data
          this.pageTotal = resData.count
          this.pageCurrent = resData.PageNum
        })
        .catch(err => {})
    },
    changePage (page) {
      this.setPageData(page)
    },
    changePageSize (pageSize) {
      this.setPageData(1, pageSize)
    },
    handleDelete (params) {
      var row = params.row
      this.$Modal.confirm({
        title: '提示',
        content: '<p>确定要删除[' + row.id + ']?</p>',
        onOk: () => {
          this.removeUser(row)
        },
        onCancel: row => {}
      })
    },
    handleAdd () {
      this.$refs.edit.handleReset()
      this.modelEdit = true
      this.isAdd = true
      this.eidtRow = {}
    },
    handleEdit (params) {
      this.modelEdit = true
      this.isAdd = false
      this.eidtRow = params.row
    },
    removeUser (row) {
      var id = row.id
      removeUser(id)
        .then(res => {
          const resData = res.data
          const data = resData.data
          const code = resData.code
          const msg = resData.msg
          if (code == 1) {
            this.$Message.info('删除成功')
            this.setPageData()
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true })
          }
        })
        .catch(err => {})
    },
    handleTakePhoto (params) {
	  this.modelPhoto = true
	  this.isAdd = false
	  this.eidtRow = params.row
    },
    saveEdit () {
      var row = this.$refs.edit.Row
    },
    openSocket () {
      console.log(this.$socket)
      this.$socket.onopen = function () {
        var serial_info = {
			    com_num: 'com3',
			    baudrate: '38400',
			    databits: '8',
			    stopbits: '1',
			    parity: 'parity'
        }
        var JSON_serial = JSON.stringify(serial_info)

        this.$socket.send(JSON_serial)
      }
    }
  },
  mounted () {
    this.setPageData(1)
  }
}
</script>
