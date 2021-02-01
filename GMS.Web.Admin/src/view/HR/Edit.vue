<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="ruleUser" :label-width="100">
      <Row>
        <Col span="12">
          <FormItem label="工号" prop="userId">
            <Input v-model="Row.userId" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="姓名" prop="userName">
            <Input v-model="Row.userName" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="12">
          <FormItem label="电话" prop="phoneNumber">
            <Input v-model="Row.phoneNumber" />
          </FormItem>
        </Col>
        <Col span="12">
			<FormItem label="过期时间" prop="expiryDate">
				<Date-picker type="date" v-model="Row.expiryDate" />
			</FormItem>
        </Col>
      </Row>
	  <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEdit=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>
  </div>
</template>
<script>
import { add as addUser, edit as editUser } from "@/api/HRUser";
import { truncate, truncateSync } from "fs";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      ruleUser: {
        userId: [
          {
            required: true,
            message: "工号必填",
            trigger: "blur"
          },
          {
            type: "string",
            min: 8,
            message: "填写8位工号",
            trigger: "blur"
          }
        ],
        userName: [
          {
            required: true,
            message: "名称必填",
            trigger: "blur"
          }
        ],
		phoneNumber: [
		  {
		    required: true,
		    message: "电话必填",
		    trigger: "blur"
		  },
		  {
			type: "string",
		    min: 11,
		    message: "填写11位手机号",
		    trigger: "blur"
		  }
		],
		expiryDate: [
		  {
		    required: true,
			type:"date",
		    message: "过期时间必填",
		    trigger: "blur"
		  }
		]
      }
    };
  },
  methods: {   
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          addUser(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit = false;
                this.parent.setPageData(1);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
          editUser(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEdit = false;
                this.parent.setPageData();
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.handleReset();
      this.Row = Object.assign({}, newVal);
    }
  }
};
</script>

