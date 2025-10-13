import { Request, Response } from 'express';
import { propertiesData, tags } from '../data';
import { IProperty, PropertyModel } from '../models/property.model';

export class PropertyController {
    public static async propertySeed(req: Request, res: Response) {
        const propertiesCount = await PropertyModel.countDocuments();

        if (propertiesCount > 0) {
            res.send("Seed is already done!");
            return;
        } 

        await PropertyModel.create(propertiesData);
        res.send("Seed is done!")
    }

    public static async registerProperty(req: Request, res: Response) {
        const { street, number, neighborhood, city, uf, country, bedroom, bathroom, kitchen, spot, area, externalArea, price, title, description, contactNumber, email, url, tags } = req.body;

        if (!street || !number || !neighborhood || !city || !uf || !country || !bedroom || !bathroom || !kitchen || !spot || !area || !externalArea || !price || !title || !description || !contactNumber || !email || !url) {
            return res.status(400).json({ message: "Campos obrigatórios estão faltando." });
        }

        const newProperty: IProperty = {
            id: '',
            street,
            number,
            neighborhood,
            city,
            uf,
            country,
            bedroom,
            bathroom,
            kitchen,
            spot,
            area,
            externalArea,
            price,
            title,
            description,
            contactNumber,
            email,
            url,
            tags
        }

        const dataProperty = await PropertyModel.create(newProperty);
        return res.status(201).json({ message: "Propriedade cadastrada com sucesso!", property: dataProperty });
    }

    public static async getAllProperties(req: Request, res: Response) {
        const properties = await PropertyModel.find();
        res.send(properties);
    }

    public static async getAllTags(req: Request, res: Response) {
        res.send(tags);
    }

    public static async getPropertyByID(req: Request, res: Response) {
        const properties = await PropertyModel.findById(req.params.idSearched);
        res.send(properties);
    }

    public static async getPropertiesByAddress(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');

        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ]
        });
        
        res.send(properties);
    }
    
    public static async getPropertiesByTag(req: Request, res: Response) {
        const properties = await PropertyModel.find({tags: req.params.tagSearched});
        res.send(properties);
    }

    public static async getPropertiesByMinimumPrice(req: Request, res: Response) {
        const properties = await PropertyModel.find({price: {$gte: req.params.minimunPrice}});
        res.send(properties);
    }

    public static async getPropertiesByMaximumPrice(req: Request, res: Response) {
        const properties = await PropertyModel.find({price: {$lte: req.params.maximunPrice}});
        res.send(properties);
    }

    public static async getPropertiesByBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({bedroom: req.params.numberOfBedrooms});
        res.send(properties);
    }
    
    public static async getPropertiesByAddressAndTag(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ]
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressAndMinimunPrice(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByAddressAndMaximunPrice(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByAddressAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }

    public static async getPropertiesByTagAndMinimunPrice(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $gte: req.params.minimunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByTagAndMaximunPrice(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByTagAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
        
    public static async getPropertiesByPriceRange(req: Request, res: Response) {
        const properties = await PropertyModel.find({price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice }});
        res.send(properties);
    }

    
    public static async getPropertiesByMinimunPriceAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            price: { $gte: req.params.minimunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByMaximunPriceAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            price: { $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }

    public static async getPropertiesByAddressTagAndMinimunPrice(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            tags: req.params.tagSearched,
            price: { $gte: req.params.minimunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByAddressTagAndMaximunPrice(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            tags: req.params.tagSearched,
            price: { $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }

    public static async getPropertiesByAddressTagAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            tags: req.params.tagSearched,
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressAndPriceRange(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }
        
    public static async getPropertiesByAddressMinimunPriceAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
        
    public static async getPropertiesByAddressMaximunPriceAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByTagAndPriceRange(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }
    
    public static async getPropertiesByTagMinimunPriceAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $gte: req.params.minimunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
        
    public static async getPropertiesByTagMaximunPriceAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
        
    public static async getPropertiesByPriceRangeAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressTagAndPriceRange(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice }
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressTagMinimunPriceAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressTagMaximunPriceAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    public static async getPropertiesByAddressPriceRangeAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }

    public static async getPropertiesByTagPriceRangeAndBedrooms(req: Request, res: Response) {
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
    
    
    public static async getPropertiesByAddressTagPriceRangeAndBedrooms(req: Request, res: Response) {
        const searchRegex = new RegExp(req.params.addressSearched, 'i');
        const properties = await PropertyModel.find({
            tags: req.params.tagSearched,
            $or: [
                { city: {$regex: searchRegex} },
                { country: {$regex: searchRegex} },
                { uf: {$regex: searchRegex} }
            ],
            price: { $gte: req.params.minimunPrice, $lte: req.params.maximunPrice },
            bedroom: req.params.numberOfBedrooms
        });
        res.send(properties);
    }
}