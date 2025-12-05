add_executable(day5)
target_sources(day5
    PRIVATE
    day5.cpp)
target_link_libraries(day5 read_file)
target_include_directories(day5 PRIVATE ../utils)
