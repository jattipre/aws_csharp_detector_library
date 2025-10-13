import {Controller, Get, Post, Req, Res, Session, UseBefore} from "@tsed/common";
import BaseController from "../../Core/BaseController";
import {Mongo} from "../../services/Mongo";
import {ifNotLoggedIn, ifNotUser} from "../../middlewares/SessionCheck";
import {Question} from "../../models/Question";
import {Notification, NotificationType} from "../../config/Notification";
import {Data} from "../../config/SessionData";
import {Plugins} from "../../config/Utility";
import {User} from "../../models/User";
import {Datatable} from "../../libraries/Datatable";
import * as moment from "moment";


@Controller("/")
@UseBefore(ifNotLoggedIn)
@UseBefore(ifNotUser)
export class Index extends BaseController {
	constructor(private  mongo: Mongo) {
		super(mongo);
		this.config.view = "userUi/pages";

	}

	@Get("/index")
	async index(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let area = await this.mongo.AreaService.find({
			deleted: false
		});
		let hotelCount = await this.mongo.HotelService.aggregate().match({
			deleted: false
		}).group({
			_id: '$area',
			count: {
				$sum: 1
			}
		});
		this.config.data['count'] = hotelCount;
		let packages = await this.mongo.PackageService.find({
			deleted: false
		});
		this.config.data['packages'] = packages;
		this.config.data['area'] = area;
		this.config.render = 'index';
		this.config.pageScript.push(Plugins.selectize, Plugins.dateRangePicker);
		await this.render(req, res);
	}

	@Post("/getArea")
	async getArea(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let areaObject = await this.mongo.AreaService.find({
			name: {$regex: new RegExp(req.body.term, 'i')},
			deleted: false,
		});
		let areas = [];
		for (const area of areaObject) {
			areas.push({id: area._id, name: area.name});
		}
		res.send(areas);
	}

	@Get("/hotelbookings")
	async hotelBookings(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {
		let bookings = await this.mongo.BookingService.find({
			deleted: false,
			user: session.user._id,
			hotelFlag: true
		});
		this.config.data['bookings'] = bookings;
		this.config.render = ('hotelBookings');
		await this.render(req, res);

	}

	@Get("/packagebookings")
	async packageBookings(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {
		let bookings = await this.mongo.BookingService.find({
			deleted: false,
			user: session.user._id,
			packageFlag: true
		});

		this.config.data['bookings'] = bookings;
		this.config.render = ('packageBookings');
		await this.render(req, res);

	}

	@Get("/contact")
	@Post("/contact")
	async contact(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {

		if (req.method == "POST") {
			let {name, email, question} = req.body;
			let contactObject = new Question();
			contactObject.name = name;
			contactObject.email = email;
			contactObject.question = question;
			let questionObject = new this.mongo.QuestionService(contactObject);
			await questionObject.save();
			let notification: Notification = {
				message: "New Offer Added successfully !",
				type: NotificationType.SUCCESS,
				title: "Added success"
			};
			req.session.notification.push(notification);
			return res.redirect('back');
		} else {
			this.config.render = ('contact');
			await this.render(req, res);
		}


	}

	@Get("/partnerTerms")
	@Post("/partnerTerms")
	async partnerTerms(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {
		if (req.method == 'POST') {
			let {name, company, property, phone, email, description} = req.body;

			let partnerRequest = new User();
			partnerRequest.name = name;
			partnerRequest.company = company;
			partnerRequest.property = property;
			partnerRequest.phone = phone;
			partnerRequest.email = email;
			partnerRequest.description = description;
			partnerRequest.partnerRequest = true;
			let partnerObject = new this.mongo.UserService(partnerRequest);
			await partnerObject.save();
			let notification: Notification = {
				message: "Request has been sent successfully !",
				type: NotificationType.SUCCESS,
				title: "Request success"
			};
			req.session.notification.push(notification);
			return res.redirect('/user/index');
		} else {
			this.config.render = ('partnerTerms');
			await this.render(req, res);
		}

	}

	@Get("/blog")
	async blog(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {
		this.config.render = ('blog');
		await this.render(req, res);

	}
}