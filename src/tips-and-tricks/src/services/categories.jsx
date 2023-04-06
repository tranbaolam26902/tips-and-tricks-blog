import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getCategoryBySlug(slug) {
	const { data } = await axios.get(`${API_URL}/categories/${slug}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getCategories() {
	const { data } = await axios.get(
		`${API_URL}/categories?PageSize=1000&PageNumber=1`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
