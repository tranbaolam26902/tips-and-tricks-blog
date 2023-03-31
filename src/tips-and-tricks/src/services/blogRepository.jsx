import axios from 'axios';

// Start: Posts
export async function getPosts(
	keyword = '',
	pageSize = 10,
	pageNumber = 1,
	sortColumn = '',
	sortOrder = '',
	published = true,
	unpublished = false,
) {
	const response = await axios.get(
		`https://localhost:7157/api/posts?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}&Published=${published}&Unpublished=${unpublished}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getPostsByQueries(queries) {
	const response = await axios.get(
		`https://localhost:7157/api/posts?${queries}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
// End: Posts

// Start: Categories
export async function getCategory(slug) {
	const response = await axios.get(
		`https://localhost:7157/api/categories/${slug}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
// End: Categories
