pub fn add(left: usize, right: usize) -> usize {
    left + right
}

#[cfg(test)]
mod tests {
    use super::*;
    use rusty_unit::*;

    #[test]
    fn it_works() {
        let result = add(2, 2);
        assert_that!(result, is::equal_to(4));
        assert_that!(result, is::not::equal_to(7));
        assert_that!(result, is::greater_than(2));
        assert_that!(result, is::not::greater_than(7));
        assert_that!(result, is::less_than(7));
        assert_that!(result, is::not::less_than(2));
    }
}
