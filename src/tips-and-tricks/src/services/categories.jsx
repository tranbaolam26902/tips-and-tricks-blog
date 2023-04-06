import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getCategoryBySlug(slug) {
	const response = await axios.get(`${API_URL}/api/categories/${slug}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getCategories() {
	const response = await axios.get(
		`${API_URL}/api/categories?PageSize=1000&PageNumber=1`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
