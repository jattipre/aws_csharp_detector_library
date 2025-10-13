import {Controller, Get, PathParams, Post, Req, Res, Session, UseBefore} from "@tsed/common";
import {ifNotAdmin, ifNotLoggedIn} from "../../middlewares/SessionCheck";
import BaseController from "../../Core/BaseController";
import {Mongo} from "../../services/Mongo";
import {Notification, NotificationType} from "../../config/Notification";
import {Area} from "../../models/Area";
import {Data} from "../../config/SessionData";
import {MakeFileProperty, METHOD} from "../../config/Config";
import {Action, TableAction} from "../../config/TableAction";
import {Datatable} from "../../libraries/Datatable";
import {Plugins, Url} from "../../config/Utility";
import Button = TableAction.Button;
import ButtonType = TableAction.ButtonType;
import makeUrl = Url.makeUrl;
import {MultipartFile} from "@tsed/multipartfiles";

@Controller("/")
@UseBefore(ifNotLoggedIn)
@UseBefore(ifNotAdmin)
export class Offer extends BaseController {
	constructor(private  mongo: Mongo) {
		super(mongo);
		this.config.view = "admin/area";
	}

	@Get("/addArea")
	@Post("/addArea")
	async addArea(@Res() res: Res, @Req() req: Req, @Session('user') user: any ,@MultipartFile('image') image: Express.Multer.File) {
		if (req.method === 'POST') {

			let {name} = req.body;
			let areaObject = new Area();
			areaObject.name=name;
			if(image){
				areaObject.image = MakeFileProperty(image);
			}

			let area = new this.mongo.AreaService(areaObject);
			await area.save();

			let notification: Notification = {
				message: "New Area Added successfully !",
				type: NotificationType.SUCCESS,
				title: "Added success"
			};
			req.session.notification.push(notification);
			return res.redirect('/admin/areas');
		} else {
			this.config.render = ('addArea');
			await this.render(req, res);
		}

	}
	@Post("/getArea")
	async getArea(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let areaObject = await this.mongo.AreaService.find({
			name: {$regex: new RegExp(req.body.term, 'i')},
			deleted: false
		});

		let areas = [];
		for (const pac of areaObject) {
			areas.push({id: pac._id, name: pac.name});
		}
		res.send(areas);
	}

	@Get('/areas')
	@Post("/areas")
	async areas(@Res() res: Res, @Req() req: Req, @Session("user")session: any) {
		if (req.method == METHOD.POST) {
			let action = new Action([
				new Button(ButtonType.SELF_LINK, makeUrl('/editArea/{$1}', session), '<i class="fas fa-edit"></i>'),
				new Button(ButtonType.DELETE, makeUrl('/deleteArea/{$1}', session), '<i class="fas fa-trash"></i>')]);

			let dt = new Datatable(this.mongo.AreaService, JSON.parse(req.body.request), {
				$and: [{
					deleted: false,
				}]
			});
			let data = await dt.generate(action);
			res.send(data);
		} else {
			this.config.render = "areas";
			this.config.pageScript.push(Plugins.dataTables, Plugins.selectize);
			await this.render(req, res);
		}
	}

	@Get('/deleteArea/:areaID')
	async deleteArea(@Res() res: Res, @Req() req: Req, @Session('user') session: Data, @PathParams('areaID') areaID: string) {
		let areaObject = await this.mongo.AreaService.findById(areaID);
		areaObject.deleted = true;
		await areaObject.save();
		let notification: Notification = {
			message: "Area Deleted successfully !",
			type: NotificationType.SUCCESS,
			title: "Delete success"
		};
		req.session.notification.push(notification);
		return res.redirect('back');
	}
	@Get("/editArea/:areaID")
	@Post("/editArea/:areaID")
	async editArea(@Res() res: Res, @Req() req: Req, @Session('user') user: any , @PathParams('areaID')areaID:string , @MultipartFile('image') image: Express.Multer.File) {
		if (req.method === 'POST') {
			let {name} = req.body;

			let areaObject = await this.mongo.AreaService.findById(areaID);
			areaObject.name=name;
			if(image){
				areaObject.image = MakeFileProperty(image);
			}
			await areaObject.save();

			let notification: Notification = {
				message: "Area Edited successfully !",
				type: NotificationType.SUCCESS,
				title: "Edit success"
			};
			req.session.notification.push(notification);
			return res.redirect('/admin/areas');
		} else {
			this.config.data['area'] = await this.mongo.AreaService.findById(areaID);
			this.config.render = ('editArea');
			await this.render(req, res);
		}

	}
}