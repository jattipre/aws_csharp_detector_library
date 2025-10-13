import {Controller, Get, PathParams, Post, Req, Res, Session, UseBefore} from "@tsed/common";
import BaseController from "../Core/BaseController";
import {Mongo} from "../services/Mongo";
import {User} from "../models/User";
import {Notification, NotificationType} from "../config/Notification";
import {Data} from "../config/SessionData";
import {ifLoggedIn, ifNotLoggedIn} from "../middlewares/SessionCheck";
import {UserType} from "../config/Config";
import {Plugins} from "../config/Utility";
import * as moment from "moment";

const bcrypt = require("bcryptjs");

@Controller("/")
export class Home extends BaseController {
	constructor(private mongo: Mongo) {
		super(mongo);
		this.config.view = "home";
	}

	@Get("/")
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


	@Get("/")
	@Get("/login")
	@Post("/login")
	@UseBefore(ifLoggedIn)
	async login(@Res() res: Res, @Req() req: Req, @Session("user") session: any) {
		if (req.method == 'POST') {
			let {email, password} = req.body;
			let User: User = await this.mongo.UserService.findOne({
				email: email,
			});
			if (!req.session) {
				req.session.notification = new Array<Notification>();
			} else {
				if (!req.session.notification) {
					req.session.notification = new Array<Notification>();
				}
			}
			if (User) {
				if (bcrypt.compareSync(password, User.password)) {
					let data = new Data();
					data.userType = User.userType;
					data.user = User;
					req.session.user = data;
					let user = await this.mongo.UserService.findById(User._id);
					await user.save();
					let notification: Notification = {
						message: "Login as " + User.name + "  @" + Date() + "  from" + req.connection.remoteAddress,
						type: NotificationType.SUCCESS,
						title: "login success!"
					};
					req.session.notification.push(notification);
					if (req.session.oldRequest) {
						let url = req.session.oldRequest;
						delete req.session.oldRequest;
						return res.redirect(url);

					}
					if (User.userType === UserType.ADMIN) {
						return res.redirect("/admin/dashboard");
					} else if (User.userType === UserType.PARTNER) {
						return res.redirect("/partner/dashboard");
					} else {
						return res.redirect("user/index");
					}
				} else {
					let notification: Notification = {
						message: "Username and password does not match!",
						type: NotificationType.WARNING,
						title: "Login Failed!"
					};
					req.session.notification.push(notification);
					return res.redirect("/login");
				}

			} else {
				let notification: Notification = {
					message: "Username does not registered!",
					type: NotificationType.WARNING,
					title: "Login Failed!"
				};
				req.session.notification.push(notification);
				return res.redirect("/login");
			}

		} else {
			this.config.render = ("login");
			await this.render(req, res);
		}
	}

	@Get("/logout")
	@UseBefore(ifNotLoggedIn)
	logout(@Req() req: Req, @Res() res: Res) {
		delete req.session.user;
		req.session.notification = [{
			message: "You have successfully sign out",
			type: NotificationType.SUCCESS,
			title: "Logout Success!"
		}];
		return res.redirect('/index');
	}

	@Get("/apply")
	@Post("/apply")
	@UseBefore(ifLoggedIn)
	async apply(@Res() res: Res, @Req() req: Req) {
		if (req.method == 'POST') {
			let {firstName, lastName, email, password, phone} = req.body;
			let user = new User();
			user.firstName = firstName;
			user.lastName = lastName;
			user.email = email;
			user.phone = phone;
			user.userType = UserType.USER;
			user.password = bcrypt.hashSync(password, 12);
			let data = new this.mongo.UserService(user);
			await data.save();
			let notification: Notification = {
				message: "Sign up Complete Success",
				type: NotificationType.SUCCESS,
				title: "Sign Up Done!"
			};
			req.session.notification.push(notification);
			return res.redirect("/login");
		} else {

			this.config.render = "apply";
			await this.render(req, res);
		}
	}

	@Get('/test')
	async test(@Req() req: Req, @Res() res: Res) {
		let user = new User();
		user.firstName = "Alkemy";
		user.lastName = "Hossain";
		user.company = "trioQuad";
		user.email = "admin@gmail.com";
		user.password = bcrypt.hashSync("2021", 12);
		user.userType = UserType.ADMIN;
		user.address = "mirpur 1, dhaka bangladesh";
		user.phone = "015558357036";

		let userService = new this.mongo.UserService(user);
		await userService.save();
	}

	// @Get('/recoverPassword')
	// @Post('/recoverPassword')
	// @UseBefore(ifLoggedIn)
	// @UseBefore(ifNotLoggedIn)
	// async recoverPassword(@Res() res: Res, @Req() req: Req,) {
	// 	console.log('in');
	// 	if(req.method==="POST"){
	// 		let{email}=req.body;
	// 		console.log(req.body);
	// 	}
	// }

	// user  pages without login***********************************************************************************************************************************************************************


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
	//Area Wise search from card



	@Get("/packages")
	async viewPackage(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let packages = await this.mongo.PackageService.find({
			deleted: false,
		});
		this.config.data['packages'] = packages;

		this.config.render = 'packages';
		this.config.pageScript.push(Plugins.selectize);
		await this.render(req, res);
	}

	@Post("/areaSearch")
	async search(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let {destination, checkIn, checkOut, rooms, adult, child} = req.body;
		let hotelObject = await this.mongo.HotelService.find({
			deleted: false,
			area: destination
		});
		this.config.data['hotels'] = hotelObject;
		this.config.render = 'hotels';
		await this.render(req, res);
	}

	@Get("/hotels")
	async hotels(@Res() res: Res, @Req() req: Req, @Session('user') session: Data) {
		let hotels = await this.mongo.HotelService.find({
			deleted: false
		});
		this.config.data['hotels'] = hotels;

		this.config.render = 'hotels';
		this.config.pageScript.push(Plugins.selectize);
		await this.render(req, res);
	}
	// area wise search
	@Get("/hotels/:areaID")
	@Post("/hotels/:areaID")
	async hotelsByArea(@Res() res: Res, @Req() req: Req, @Session('user') session: Data, @PathParams('areaID') areaID: string) {
		let hotelsByArea = await this.mongo.HotelService.find({
			deleted: false,
			area: areaID
		});
		let hotelCount = await this.mongo.HotelService.countDocuments({
			deleted: false,
			area: areaID
		});

		this.config.data['count'] = hotelCount;
		this.config.data['hotels'] = hotelsByArea;

		this.config.render = 'hotels';
		this.config.pageScript.push(Plugins.selectize);
		await this.render(req, res);
	}

	@Get("/contact")
	@UseBefore(ifLoggedIn)
	async contact(@Res() res: Res, @Req() req: Req) {
		this.config.render = "contact";
		await this.render(req, res);
	}
	@Get("/blog")
	async blog(@Res() res: Res, @Req()req: Req, @Session('user') session: Data,) {
		this.config.render = ('blog');
		await this.render(req, res);

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
			return res.redirect('/partnerTerms');
		} else {
			this.config.render = ('partnerTerms');
			await this.render(req, res);
		}

	}

	@Get('/hotelPreview/:hotelID')
	async deleteOffer(@Res() res: Res, @Req() req: Req, @Session('user') session: Data, @PathParams('hotelID') hotelID: string) {
		let hotelObject = await this.mongo.HotelService.findById(hotelID);
		let available = await this.mongo.BookingService.aggregate()
			.match({
				deleted: false,
				checkIn: {
					$gte: moment().startOf('days').toDate(),
					//$lt:
				},
				checkOut: {
					$gte: moment().endOf('days').toDate(),
					//$lt:
				}
			}).group({
				_id: '$rooms',
				count: {
					$sum: '$quantity'
				}
			});
		this.config.data['availables'] = available;
		this.config.data['hotel'] = hotelObject;
		this.config.render = 'hotelPreview';
		this.config.pageScript.push(Plugins.selectize);
		await this.render(req, res);
	}

	@Get('/packagePreview/:packageID')
	@Post('/packagePreview/:packageID')
	async packagePreview(@Res() res: Res, @Req() req: Req, @Session('user') session: Data, @PathParams('packageID') packageID: string) {
		let packageObject = await this.mongo.PackageService.findById(packageID);
		this.config.data['package']=packageObject;
		this.config.render='packagePreview';
		this.config.pageScript.push(Plugins.selectize);
		await this.render(req, res);
	}





}
