import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getCategories() {
	const response = await axios.get(
		`${API_URL}/api/categories?PageSize=10&PageNumber=1`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getFeaturedPosts(limit) {
	const response = await axios.get(`${API_URL}/api/posts/featured/${limit}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getRandomPosts(limit) {
	const response = await axios.get(`${API_URL}/api/posts/random/${limit}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getTagCloud() {
	const response = await axios.get(
		`${API_URL}/api/tags?PageSize=100&PageNumber=1`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getTopAuthors(limit) {
	const response = await axios.get(`${API_URL}/api/authors/best/${limit}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getArchives(limit) {
	const response = await axios.get(`${API_URL}/api/posts/archive/${limit}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
